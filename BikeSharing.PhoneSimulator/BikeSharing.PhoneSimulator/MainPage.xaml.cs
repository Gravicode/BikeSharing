using BikeSharing.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using Xamarin.Forms;
using Xamarin.Essentials;
using Location = Xamarin.Essentials.Location;

namespace BikeSharing.PhoneSimulator
{
    public partial class MainPage : ContentPage
    {
        static int TimerCountdown;
        static string DeviceName = "KayuhBike1";
        static TripInfo Info;
        static bool LastSOS = false;
        static GpsPoint LastPosition = null;
        static bool IsStopped=false;

        const string MQTT_BROKER_ADDRESS = "13.76.142.227";
        public static MqttClient client { set; get; }
        string clientId = Guid.NewGuid().ToString();
        string username = "mifmasterz";
        string password = "123qweasd";
        string DataTopic = "mifmasterz/BMC/data";
        string ControlTopic = "mifmasterz/BMC/control";
        public MainPage()
        {
            InitializeComponent();
            BtnSoS.IsEnabled = false;
            BtnSoS.Clicked += BtnSoS_Clicked;
            TxtDeviceName.Text = DeviceName;
            BtnStart.Clicked += BtnStart_Clicked;
        }

        private void BtnStart_Clicked(object sender, EventArgs e)
        {
            BtnStart.IsEnabled = false;
            var th1 = new Thread(new ThreadStart(TelemetryLoop));
            th1.Start();
            BtnSoS.IsEnabled = true;
        }

        private void BtnSoS_Clicked(object sender, EventArgs e)
        {
            LastSOS = true;
        }

        void CountDown()
        {
            while (true)
            {
                if (TimerCountdown > 0)
                {
                    TimerCountdown--;
                    var ts = new TimeSpan(0, 0, TimerCountdown);
                    UpdateTimer("Countdown timer : " + ts.Hours + ":" + ts.Minutes + ":" + ts.Seconds);
                }
                else
                {
                    UpdateTimer("Time is up, please return this bike to station..");
                }
                Thread.Sleep(1000);
                if (Info != null && !Info.IsActive) break;
            }
        }

        async void TelemetryLoop()
        {
         
            client = new MqttClient(IPAddress.Parse(MQTT_BROKER_ADDRESS));

            client.Connect(clientId, username, password);

            SubscribeMessage();
            Random rnd = new Random();
            while (true)
            {
                try
                {
                    Device.BeginInvokeOnMainThread(async() => {
                        var location = await Geolocation.GetLastKnownLocationAsync();

                        if (location != null)
                        {
                            LastPosition = new GpsPoint() { Latitude = location.Latitude, Longitude = location.Longitude, Timestamp = DateTime.Now };
                            Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                        }
                    });
                   
                }
                catch (FeatureNotSupportedException fnsEx)
                {
                    // Handle not supported on device exception
                    WriteLine("geature gps not supported");
                }
                catch (PermissionException pEx)
                {
                    // Handle permission exception
                    WriteLine("butuh permission gps");
                }
                catch (Exception ex)
                {
                    // Unable to get location
                    WriteLine("unable to get location");
                }

                if (Info != null && (Info.IsActive || IsStopped))
                {
                    var item = new DeviceData() { Info = Info, TimeStamp = DateTime.Now, IsLocked = IsStopped ? true : false, Position = LastPosition == null ? new GpsPoint() { Latitude = 0, Longitude = 0 } : LastPosition, SOS = LastSOS };
                    LastSOS = false;
                    IsStopped = false;
                    PublishMessage(DataTopic,JsonConvert.SerializeObject(item));
                }
                Thread.Sleep(2000);

            }
            client.Disconnect();
            //loop forever



        }

        private void DoAction(string data)
        {
            //var objX = Json.NETMF.JsonSerializer.DeserializeString(data) as DictionaryEntry;
            var action = JsonConvert.DeserializeObject<DeviceAction>(data);// { ActionName = hashTable["ActionName"].ToString(), Params0 = hashTable["Params0"] == null ? "" : hashTable["Params0"].ToString(), Params1 = hashTable["Params1"] == null ? "" : hashTable["Params1"].ToString() };
            // Check the payload is a single integer value
            if (action != null)
            {

                switch (action.ActionName)
                {
                    case "Start":
                        //unlock
                        if (Info != null && Info.IsActive)
                        {
                            WriteLine("Bike is already started / unlocked");
                            break;
                        }
                        else
                        {
                            WriteLine("Bike is unlocked/started");
                           
                        }
                        TimerCountdown = 30 * 60;
                        Info = new TripInfo();
                        Info.TripNumber = action.Params0;
                        Info.UserName = action.Params1;
                        Info.StartDate = DateTime.Now;
                        Info.EndDate = DateTime.Now;
                        Info.DeviceName = DeviceName;
                        Info.IsActive = true;
                        var CountDownThread = new Thread(new ThreadStart(CountDown));
                        CountDownThread.Start();

                        break;
                    case "Stop":
                        //lock
                        if (Info != null && Info.IsActive)
                        {
                            Info.EndDate = DateTime.Now;
                            Info.IsActive = false;
                            
                            //var item = new DeviceData() { TimeStamp = DateTime.Now, Info = Info, SOS = LastSOS, Position = LastPosition };
                            IsStopped = true;
                            WriteLine("Bike is stopped/locked");
                        }
                        else
                        {
                            WriteLine("Bike is already stopped/locked");

                        }
                        break;
                    case "SOS":
                        LastSOS = true;
                        WriteLine("sos is turned on");
                        break;

                }


            }

        }

      
        void PublishMessage(string Topic, string Pesan)
        {
            client.Publish(Topic, Encoding.UTF8.GetBytes(Pesan), MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, false);
            WriteLine("SENDING: " + DateTime.Now.ToString("HH:mm:ss"));
        }

        void SubscribeMessage()
        {
            // register to message received 
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            client.Subscribe(new string[] { DataTopic, ControlTopic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });

        }

        void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {

            // handle message received 
            string Message = new string(Encoding.UTF8.GetChars(e.Message));

            Debug.Print("Message Received : " + Message);
            if (e.Topic == ControlTopic)
            {
                DoAction(Message);
            }

        }

        void UpdateTimer(string Message, bool Status = false)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                TxtTimer.Text = Message;
            });
        }

        void WriteLine(string Message, bool Status = false)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                TxtStatus.Text = Message;
            });
            
        }

       
    }
}
