using BikeSharing.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ServiceStack.Redis;
using System;
using System.IO;
using System.Text;
using System.Threading;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

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

        static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
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
            }
        }
    }
}
