using BikeSharing.MobileApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BikeSharing.MobileApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {
        static int CountDown;
        Timer timer;
        string DeviceName = "KayuhBike1";
        static AzureIoT iot = new AzureIoT();
        public StartPage()
        {
            InitializeComponent();
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
                        await iot.InvokeMethod(DeviceName, "Start", new string[] { GenerateTripNumber(), TxtUserName.Text });
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
                await iot.InvokeMethod(DeviceName, "Stop", new string[] { });
            };
            BtnSOS.Clicked += async (object sender, EventArgs e) =>
            {
                await iot.InvokeMethod(DeviceName, "SOS", new string[] { });
            };
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
            string TripNo = $"{DateTime.Now.ToString("YYYY/MM/DD/HH:mm:ss")}-{DeviceName}-{shortid.ShortId.Generate(10).ToUpper()}";
            return TripNo;
        }
        
    }
}