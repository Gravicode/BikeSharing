using BikeSharing.Models;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BikeSharing.BikeSimulator
{
    class Program
    {

        static bool IsSOS = false;
        static bool IsConnected = false;
        static HttpClient client;
        static int TimerCountdown;
        static string DeviceName = "KayuhBike1";
        static TripInfo Info;
        static Task CountDownTask;
        static void Main(string[] args)
        {
            Console.WriteLine("Device is up!");
            Setup();
            StartTimer();

            Console.ReadLine();
        }

        static Task CountDown()
        {
            while (true)
            {
                if (TimerCountdown > 0)
                {
                    TimerCountdown--;
                    var ts = new TimeSpan(0, 0, TimerCountdown);
                    Console.WriteLine($"Countdown timer : {ts.ToString("HH:mm:ss")}");
                    Console.WriteLine("----------------------------");
                }
                else
                {
                    Console.WriteLine("Time is up, please return this bike to station..");
                }
                Thread.Sleep(1000);
            }
        }
        static Task StartTimer()
        {
            while (true)
            {
                try
                {
                    if (IsConnected)
                    {
                        if (Info != null && Info.IsActive)
                        {

                            var item = new DeviceData(0, 0) { TimeStamp = DateTime.Now, Info = Info, SOS = IsSOS, Position = new Location() { Latitude = 0, Longitude = 0 } };
                            SendDeviceToCloudMessagesAsync(item);
                            IsSOS =false;
                            Console.WriteLine($"send data at {DateTime.Now}");
                            Console.WriteLine("----------------------------");
                        }
                    }
                    else
                    {
                        Setup();
                    }
                }
                catch
                {
                    IsConnected = false;
                }
                Thread.Sleep(3000);
            }
        }
        static void Setup()
        {
            try
            {
                if (!IsConnected)
                {
                    if (s_deviceClient != null)
                    {
                        s_deviceClient.Dispose();
                    }
                    // Connect to the IoT hub using the MQTT protocol
                    s_deviceClient = DeviceClient.CreateFromConnectionString(s_connectionString, TransportType.Mqtt);
                    s_deviceClient.SetMethodHandlerAsync("DoAction", DoAction, null).Wait();
                    //SendDeviceToCloudMessagesAsync();
              


                    IsConnected = true;
                }
                if (client == null)
                {
                    client = new HttpClient();
                }
            }
            catch
            {

            }


        }
        // Handle the direct method call
        private static async Task<MethodResponse> DoAction(MethodRequest methodRequest, object userContext)
        {
            var data = Encoding.UTF8.GetString(methodRequest.Data);
            var action = JsonConvert.DeserializeObject<DeviceAction>(data);
            // Check the payload is a single integer value
            if (action != null)
            {
                /*
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Telemetry interval set to {0} seconds", data);
                Console.ResetColor();
                */
                switch (action.ActionName)
                {
                    case "Start":
                        //unlock
                        if (Info != null && Info.IsActive)
                        {
                            Console.WriteLine("Bike is already started / unlocked");
                            break;
                        }
                        TimerCountdown = 30*60;
                        Info = new TripInfo();
                        Info.TripNumber = action.Params[0];
                        Info.UserName = action.Params[1];
                        Info.StartDate = DateTime.Now;
                        Info.EndDate = DateTime.Now;
                        Info.DeviceName = DeviceName;
                        Info.IsActive = true;
                        CountDownTask = CountDown();
                        CountDownTask.Start();
                        Console.WriteLine("Bike is unlocked/started");
                        break;
                    case "Stop":
                        //lock
                        if (Info != null && Info.IsActive)
                        {
                            Info.EndDate = DateTime.Now;
                            Info.IsActive = false;
                            var item = new DeviceData(0, 0) { TimeStamp = DateTime.Now, Info = Info, SOS = IsSOS, Position = new Location() { Latitude = 0, Longitude = 0 } };
                            SendDeviceToCloudMessagesAsync(item);
                            if(CountDownTask != null)
                            {
                                CountDownTask.Dispose();
                            }
                            Console.WriteLine("Bike is stopped/locked");
                        }
                        break;
                    case "SOS":
                        IsSOS = true;
                        Console.WriteLine("sos is turned on");
                        break;
                   
                }
                // Acknowlege the direct method call with a 200 success message
                string result = "{\"result\":\"Executed direct method: " + methodRequest.Name + "\"}";
                return new MethodResponse(Encoding.UTF8.GetBytes(result), 200);
            }
            else
            {
                // Acknowlege the direct method call with a 400 error message
                string result = "{\"result\":\"Invalid parameter\"}";
                return new MethodResponse(Encoding.UTF8.GetBytes(result), 400);
            }
        }

        private static DeviceClient s_deviceClient;

        // The device connection string to authenticate the device with your IoT hub.
        // Using the Azure CLI:
        // az iot hub device-identity show-connection-string --hub-name {YourIoTHubName} --device-id MyDotnetDevice --output table
        private readonly static string s_connectionString = "HostName=BikeSharingHub.azure-devices.net;DeviceId=KayuhBike1;SharedAccessKey=dU3KqnKDWuNETEKDjkE6eBVRtesqtY3yeqD1WjD+FFQ=";

        // Async method to send simulated telemetry
        private static async void SendDeviceToCloudMessagesAsync(DeviceData data)
        {
            var message = new Message(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data)));

            // Add a custom application property to the message.
            // An IoT hub can filter on these properties without access to the message body.
            message.Properties.Add("sosAlert", (data.SOS) ? "true" : "false");

            // Send the telemetry message
            await s_deviceClient.SendEventAsync(message);
            Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, "ok");

        }

        static async Task ReceiveCommands(DeviceClient deviceClient)
        {
            Console.WriteLine("\nDevice waiting for commands from IoTHub...\n");
            Message receivedMessage;
            string messageData;

            while (true)
            {
                receivedMessage = await deviceClient.ReceiveAsync(TimeSpan.FromSeconds(1));

                if (receivedMessage != null)
                {
                    messageData = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                    Console.WriteLine("\t{0}> Received message: {1}", DateTime.Now.ToLocalTime(), messageData);

                    await deviceClient.CompleteAsync(receivedMessage);
                }
            }
        }
    }
}
