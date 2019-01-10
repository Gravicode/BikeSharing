using BikeSharing.MobileApp.Helpers;
using BikeSharing.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using BikeSharing.MobileApp.Pages;
using static Microsoft.Azure.Amqp.Serialization.SerializableType;

namespace BikeSharing.MobileApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {
        const string MQTT_BROKER_ADDRESS = "13.76.142.227";
        public static MqttClient client { set; get; }
        string clientId = Guid.NewGuid().ToString();
        string username = "mifmasterz";
        string password = "123qweasd";
        string DataTopic = "mifmasterz/BMC/data";
        string ControlTopic = "mifmasterz/BMC/control";

        static int CountDown;
        Timer timer;
        string DeviceName = "KayuhBike1";
        //static AzureIoT iot = new AzureIoT();
        
        public StartPage()
        {
            InitializeComponent();
            client = new MqttClient(IPAddress.Parse(MQTT_BROKER_ADDRESS));

            client.Connect(clientId, username, password);

            SubscribeMessage();

            timer = new Timer();
            timer.Enabled = false;
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
            BtnStart.Clicked += async (object sender, EventArgs e) =>
            {
                try
                {
                    var scanner = DependencyService.Get<IQrScanningService>();
                    var result = await scanner.ScanAsync();
                    if (result != null)
                    {
                        DeviceName = result;
                        var act = new DeviceAction() { ActionName = "Start", Params0 = GenerateTripNumber(), Params1 = TxtUserName.Text  };
                        client.Publish(ControlTopic, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(act)), MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, false);
                        CountDown = 30 * 60;
                        timer.Start();
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }
            };
            BtnStop.Clicked += async (object sender, EventArgs e) =>
            {
                var act = new DeviceAction() { ActionName = "Stop", Params0="" };
                client.Publish(ControlTopic, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(act)), MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, false);
                timer.Stop();

            };
            BtnSOS.Clicked += async (object sender, EventArgs e) =>
            {
               var act = new DeviceAction() { ActionName = "SOS", Params0 = "" };
                client.Publish(ControlTopic, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(act)), MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, false);
            };
            BtnST1.Clicked += BtnST1_Clicked;
            async void BtnST1_Clicked(object sender, EventArgs e)
            { 
                await Navigation.PushAsync(new MapPage() { Title = "Location" });
            }

        }
        
        void PublishMessage(string Topic, string Pesan)
        {
            client.Publish(Topic, Encoding.UTF8.GetBytes(Pesan), MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, false);
           
        }

        void SubscribeMessage()
        {
            // register to message received 
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            client.Subscribe(new string[] {ControlTopic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });

        }

        void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {

            // handle message received 
            string Message = new string(Encoding.UTF8.GetChars(e.Message));

            Console.WriteLine("Message Received : " + Message);
           

        }
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CountDown --;
            var ts = new TimeSpan(0, 0, CountDown);
            Device.BeginInvokeOnMainThread(() =>
            {
                TxtTimer.Text = $"{ts.Hours}:{ts.Minutes}:{ts.Seconds}";
            });
            if (CountDown <= 0)
            {
                TxtTimer.Text = "00:00:00 - Time is Up !";
                timer.Stop();
            }
           
        }

        string GenerateTripNumber()
        {
            string TripNo = $"{DateTime.Now.ToString("yyyy/MM/dd-HH:mm:ss")}-{DeviceName}-{shortid.ShortId.Generate(10).ToUpper()}";
            return TripNo;
        }
        
    }
}