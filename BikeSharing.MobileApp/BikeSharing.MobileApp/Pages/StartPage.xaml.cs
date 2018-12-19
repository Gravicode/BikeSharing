using BikeSharing.MobileApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BikeSharing.MobileApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {
        string DeviceName = "KayuhBike1";
        static AzureIoT iot = new AzureIoT();
        public StartPage()
        {
            InitializeComponent();
            BtnStart.Clicked += async (object sender, EventArgs e) =>
            {
                try
                {
                    var scanner = DependencyService.Get<IQrScanningService>();
                    var result = await scanner.ScanAsync();
                    if (result != null)
                    {
                        DeviceName = result;
                        await iot.InvokeMethod(DeviceName, "Start", new string[] { GenerateTripNumber(), TxtUserName.Text });
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }
            };
            BtnStop.Clicked += async (object sender, EventArgs e) =>
            {
                await iot.InvokeMethod(DeviceName, "Stop", new string[] { });
            };
            BtnSOS.Clicked += async (object sender, EventArgs e) =>
            {
                await iot.InvokeMethod(DeviceName, "SOS", new string[] { });
            };
        }


        string GenerateTripNumber()
        {
            string TripNo = $"{DateTime.Now.ToString("YYYY/MM/DD/HH:mm:ss")}-{DeviceName}-{shortid.ShortId.Generate(5)}";
            return TripNo;
        }
        
    }
}