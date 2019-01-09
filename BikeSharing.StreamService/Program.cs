using BikeSharing.Models;
using BikeSharing.StreamService.Helpers;
using BikeSharing.Tools;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ServiceStack.Redis;
using System;
using System.IO;
using System.Text;
using System.Threading;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.StreamService
{
    class Program
    {
       
        public static IConfigurationRoot Configuration;
        public static MqttClient mqttClient
        {
            get;
            set;
        }
        public static string mqtt_topic_server
        {
            get;
            set;
        }
        public static string mqtt_topic_data
        {
            get;
            set;
        }
        public static string rediscon
        {
            get;
            set;
        }

        public static IRedisClient redisClient
        {
            get;
            set;
        }
        static void Main(string[] args)
        {
      
            Console.OutputEncoding = Encoding.UTF8;

            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (String.IsNullOrWhiteSpace(environment))
                throw new ArgumentNullException("Environment not found in ASPNETCORE_ENVIRONMENT");

            Console.WriteLine("Environment: {0}", environment);


            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                .AddJsonFile("appsettings.json", optional: true);
            if (environment == "Development")
            {

                builder
                    .AddJsonFile(
                        Path.Combine(AppContext.BaseDirectory, string.Format("..{0}..{0}..{0}", Path.DirectorySeparatorChar), $"appsettings.{environment}.json"),
                        optional: true
                    );
            }
            else
            {
                builder
                    .AddJsonFile($"appsettings.{environment}.json", optional: true);
            }
            //add env vars
            //builder.AddEnvironmentVariables();
            //get config
            Configuration = builder.Build();

            rediscon = Configuration.GetConnectionString("RedisCon");
            var mqtt_host = Configuration.GetSection("server").GetSection("mqtt-host").Value;
            var mqtt_user = Configuration.GetSection("server").GetSection("mqtt-user").Value;
            var mqtt_pass = Configuration.GetSection("server").GetSection("mqtt-pass").Value;
            //var mqtt_devicename = Configuration.GetSection("server").GetSection("mqtt-devicename").Value;
            mqtt_topic_server = Configuration.GetSection("server").GetSection("mqtt-topic-server").Value;
            mqtt_topic_data = Configuration.GetSection("server").GetSection("mqtt-topic-broadcast").Value;
            var sqlConnectionString = Configuration.GetConnectionString("MySqlCon");

            var bikeDB = new BikeSharingDB(sqlConnectionString);
            ObjectContainer.Register<BikeSharingDB>(bikeDB);
            bikeDB.Database.EnsureCreated();
            //init redis
            var redisManager = new PooledRedisClientManager(1, rediscon);
            redisClient = redisManager.GetClient();

            //init mqtt
            var client_name = "stream-service";
            // create client instance
            mqttClient = new MqttClient(mqtt_host);

            // register to message received
            mqttClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            //string clientId = Guid.NewGuid().ToString();
            mqttClient.Connect(client_name, mqtt_user, mqtt_pass);

            // subscribe to the topic "/home/temperature" with QoS 2
            mqttClient.Subscribe(new string[] { mqtt_topic_data }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
            // Sync Service based on GRPC
            Console.WriteLine("Mqtt Service is Ready...");

            Console.ReadLine();
        }

        static async void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            if (e.Topic == mqtt_topic_data)
            {
                //save to redis

                string JsonStr = System.Text.Encoding.Default.GetString(e.Message);
                Console.WriteLine($"rec data : {JsonStr}");
                var Data = JsonConvert.DeserializeObject<DeviceData>(JsonStr);
                var redisAs = redisClient.As<DeviceData>();
                Data.Id = redisAs.GetNextSequence();
                redisAs.Store(Data);
                await Logic.InsertDeviceTelemetry(Data);
            }
        }
    }

    public class Logic
    {
        public static async Task<bool> InsertDeviceTelemetry(DeviceData data)
        {
            try
            {
                var db = ObjectContainer.Get<BikeSharingDB>();
                var exist = db.TripHeaders.Any(x => x.TripNumber == data.Info.TripNumber);
                if (!exist)
                {
                    //add new record
                    db.TripHeaders.Add(new TripHeader() { BikeNo = data.Info.DeviceName, StartDate = DateTime.Now, EndDate = DateTime.Now, IsActive=true, Latitude = data.Position.Latitude, Longitude=data.Position.Longitude, TripNumber=data.Info.TripNumber, TotalDistanceMeter=0, UserName=data.Info.UserName, Duration=new TimeSpan(0,0,0) });                  
                    db.TripDetails.Add(new TripDetail() { Latitude =data.Position.Latitude, BearingInDegrees=data.Position.BearingInDegrees, Longitude=data.Position.Longitude, SpeedInKnots=data.Position.SpeedInKnots, Timestamp = DateTime.Now, TripNumber = data.Info.TripNumber });
                    await db.SaveChangesAsync();
                }
                else
                {
                    var TripDetailNew = new TripDetail() { Latitude = data.Position.Latitude, BearingInDegrees = data.Position.BearingInDegrees, Longitude = data.Position.Longitude, SpeedInKnots = data.Position.SpeedInKnots, Timestamp = DateTime.Now, TripNumber = data.Info.TripNumber };
                    //jika berhenti
                    if (data.IsLocked)
                    {
                        var item = db.TripHeaders.Where(x => x.TripNumber == data.Info.TripNumber).FirstOrDefault();
                        item.EndDate = DateTime.Now;
                        item.IsActive = false;
                        item.Duration = item.EndDate - item.StartDate;
                        var routes = db.TripDetails.Where(x => x.TripNumber == data.Info.TripNumber).OrderBy(y => y.Id).ToList();
                        var TotalInM = 0.0;
                        for (int i = 0; i < routes.Count; i++)
                        {
                            var next = i < routes.Count-1 ? routes[i + 1] : TripDetailNew;
                            TotalInM+= GeoDistanceCalculator.DistanceInKilometers(routes[i].Latitude, routes[i].Longitude, next.Latitude, next.Longitude)*1000;
                        }
                        item.TotalDistanceMeter = TotalInM;
                    }
                    db.TripDetails.Add(TripDetailNew);
                    await db.SaveChangesAsync();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
    public static class GeoDistanceCalculator
    {
        private const double _earthRadiusInMiles = 3956.0;
        private const double _earthRadiusInKilometers = 6367.0;
        public static double DistanceInMiles(double lat1, double lng1, double lat2, double lng2)
        {
            return Distance(lat1, lng1, lat2, lng2, _earthRadiusInMiles);
        }
        public static double DistanceInKilometers(double lat1, double lng1, double lat2, double lng2)
        {
            return Distance(lat1, lng1, lat2, lng2, _earthRadiusInKilometers);
        }
        private static double Distance(double lat1, double lng1, double lat2, double lng2, double radius)
        {
            // Implements the Haversine formulat http://en.wikipedia.org/wiki/Haversine_formula
            //
            var lat = NumericExtensions.ToRadians(lat2 - lat1);
            var lng = NumericExtensions.ToRadians(lng2 - lng1);
            var sinLat = System.Math.Sin(0.5 * lat);
            var sinLng = System.Math.Sin(0.5 * lng);
            var cosLat1 = System.Math.Cos(NumericExtensions.ToRadians(lat1));
            var cosLat2 = System.Math.Cos(NumericExtensions.ToRadians(lat2));
            var h1 = sinLat * sinLat + cosLat1 * cosLat2 * sinLng * sinLng;
            var h2 = System.Math.Sqrt(h1);
            var h3 = 2 * System.Math.Asin(System.Math.Min(1, h2));
            return radius * h3;
        }
    }
    public static class NumericExtensions
    {
        public static double ToRadians(this double val)
        {
            return (System.Math.PI / 180) * val;
        }
    }

}
