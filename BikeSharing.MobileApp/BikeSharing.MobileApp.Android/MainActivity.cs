using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace BikeSharing.MobileApp.Droid
{
    [Activity(Label = "BikeSharing.MobileApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.FormsMaps.Init(this, savedInstanceState);

            var width = Resources.DisplayMetrics.WidthPixels;
            var height = Resources.DisplayMetrics.HeightPixels;
            var density = Resources.DisplayMetrics.Density;

            App.ScreenWidth = (width - 0.5f) / density;
            App.ScreenHeight = (height - 0.5f) / density;

            ZXing.Mobile.MobileBarcodeScanner.Initialize(Application);
            LoadApplication(new App());
        }
        public override void OnBackPressed()
        {
            base.OnBackPressed();
        }

    }
}