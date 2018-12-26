using BikeSharing.Models;
using Microsoft.Azure.Devices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BikeSharing.MobileApp.Helpers
{
    public class AzureIoT
    {
        private static ServiceClient s_serviceClient;
        public AzureIoT()
        {

            s_serviceClient = ServiceClient.CreateFromConnectionString(s_connectionString);

        }
        // Connection string for your IoT Hub
        // az iot hub show-connection-string --hub-name {your iot hub name}
        private readonly static string s_connectionString = "HostName=BikeSharingHub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=4zHBaY7r6j9auiKPXielNPiURw+9v6/PEt93s/j4JTQ=";
        
        // Invoke the direct method on the device, passing the payload
        public async Task InvokeMethod(string DeviceId, string ActionName = "PlaySound", params string[] Params)
        {
            var methodInvocation = new CloudToDeviceMethod("DoAction") { ResponseTimeout = TimeSpan.FromSeconds(30) };
            var action = new DeviceAction() { ActionName = ActionName, Params0 = Params[0] };
            methodInvocation.SetPayloadJson(JsonConvert.SerializeObject(action));

            // Invoke the direct method asynchronously and get the response from the simulated device.
            var response = await s_serviceClient.InvokeDeviceMethodAsync(DeviceId, methodInvocation);

            Console.WriteLine("Response status: {0}, payload:", response.Status);
            Console.WriteLine(response.GetPayloadAsJson());
        }
        

    }
}
