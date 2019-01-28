using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Maps;
using System.Diagnostics;
using Plugin.Geolocator;
using uPLibrary.Networking.M2Mqtt;
using BikeSharing.Models;
using Newtonsoft.Json;
using uPLibrary.Networking.M2Mqtt.Messages;
using Plugin.Geolocator.Abstractions;
using Position = Xamarin.Forms.Maps.Position;

namespace BikeSharing.MobileApp.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MapPage : ContentPage
	{
        Map map;
        Geocoder geocode;

        //const string MQTT_BROKER_ADDRESS = "13.76.142.227";
        //public static MqttClient client { set; get; }
        //string clientId = Guid.NewGuid().ToString();
        //string username = "mifmasterz";
        //string password = "123qweasd";
        //string DataTopic = "mifmasterz/BMC/data";
        //string ControlTopic = "mifmasterz/BMC/control";
        //string DeviceName = "KayuhBike1";
        public MapPage ()
		{
            try
            {
                Title = "Maps";
                map = new Map
                {
                    IsShowingUser = true,
                    HeightRequest = 100,
                    WidthRequest = 960,
                    VerticalOptions = LayoutOptions.FillAndExpand

                };
                loc();
                async void loc()
                {
                    var locator = CrossGeolocator.Current;
                    var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));

                    map.MoveToRegion(MapSpan.FromCenterAndRadius(
                    new Position(position.Latitude, position.Longitude/*-6.601361, 106.805080*/), Distance.FromMiles(3)));
                }

                // add the slider
                var slider = new Slider(1, 18, 1);
                slider.ValueChanged += (sender, e) =>
                {
                    var zoomLevel = e.NewValue; // between 1 and 18
                    var latlongdegrees = 360 / (Math.Pow(2, zoomLevel));
                    Debug.WriteLine(zoomLevel + " -> " + latlongdegrees);
                    if (map.VisibleRegion != null)
                        map.MoveToRegion(new MapSpan(map.VisibleRegion.Center, latlongdegrees, latlongdegrees));
                };

                //PIN
                //var fortMasonPosition = new Position(37.8044866, -122.4324132);         
                var station1 = new Position(-6.601361, 106.805080);               
                var pin = new Pin
                {
                    Type = PinType.Place,
                    Position = station1,
                    Label = "Station 1",
                    Address = "Tugu Kujang"

                };
                var station2 = new Position(-6.592302, 106.800424);
                var pin2 = new Pin
                {
                    Type = PinType.Place,
                    Position = station2,
                    Label = "Station 2",
                    Address = "Sempur"
                };
                var station3 = new Position(-6.598212, 106.794193);
                var pin3 = new Pin
                {
                    Type = PinType.Place,
                    Position = station3,
                    Label = "Station 3",
                    Address = "Pustaka"
                };
                map.Pins.Add(pin);
                map.Pins.Add(pin2);
                map.Pins.Add(pin3);

                // create map style buttons
                //var street = new Button { Text = "Street" };
                //var hybrid = new Button { Text = "Hybrid" };
                //var satellite = new Button { Text = "Satellite" };
                //street.Clicked += HandleClicked;
                //hybrid.Clicked += HandleClicked;
                //satellite.Clicked += HandleClicked;
                var segments = new StackLayout
                {
                    Spacing = 10,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Orientation = StackOrientation.Horizontal,
                    //Children = { street, hybrid, satellite }
                };

                var btnRent = new Button
                {
                    VerticalOptions = LayoutOptions.EndAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Margin = new Thickness(30, 30, 30, 30),
                    BorderRadius = 10,
                    WidthRequest = 100,
                    Text = "Rent Bike",
                };
                btnRent.Clicked += btnRent_Clicked;
                async void btnRent_Clicked(object sender, EventArgs e)
                {
                    await Navigation.PushAsync(new StartPage());
                }

                //popups
                var btnSos = new Button                
                {
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Margin = new Thickness(30, 30, 30, 30),
                    BorderRadius = 10,
                    WidthRequest = 100,
                    Text = "!!!"
                };
                btnSos.Clicked += btnSos_Clicked;
                async void btnSos_Clicked(object sender, EventArgs e)
                {

                    var action = await DisplayActionSheet("Apa ada masalah?", null, "Cancel", "Sepeda Rusak", "Sepeda Tidak Bisa Terkunci");
                    Debug.WriteLine("Action: " + action, "Location: " + station1.Latitude.ToString());
                    //var act = new SoSReport() { DeviceName = DeviceName, Message = "" };
                    //client.Publish(ControlTopic, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(act)), MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, false);
                }

                // put the page together
                var stack = new Grid {  };
                stack.Children.Add(map);
                stack.Children.Add(btnSos);
                stack.Children.Add(segments);
                stack.Children.Add(btnRent);
                Content = stack;


                // for debugging output only
                map.PropertyChanged += (sender, e) =>
                {
                    Debug.WriteLine(e.PropertyName + " just changed!");
                    if (e.PropertyName == "VisibleRegion" && map.VisibleRegion != null)
                        CalculateBoundingCoordinates(map.VisibleRegion);
                };
            }
            catch (Exception ec)
            {
                Debug.WriteLine(ec);
            }
        }

        //void HandleClicked(object sender, EventArgs e)
        //{
        //    var b = sender as Button;
        //    switch (b.Text)
        //    {
        //        case "Street":
        //            map.MapType = MapType.Street;
        //            break;
        //        case "Hybrid":
        //            map.MapType = MapType.Hybrid;
        //            break;
        //        case "Satellite":
        //            map.MapType = MapType.Satellite;
        //            break;
        //    }
        //}



        /// <summary>
        /// In response to this forum question http://forums.xamarin.com/discussion/22493/maps-visibleregion-bounds
        /// Useful if you need to send the bounds to a web service or otherwise calculate what
        /// pins might need to be drawn inside the currently visible viewport.
        /// </summary>
        static void CalculateBoundingCoordinates(MapSpan region)
        {
            // WARNING: I haven't tested the correctness of this exhaustively!
            var center = region.Center;
            var halfheightDegrees = region.LatitudeDegrees / 2;
            var halfwidthDegrees = region.LongitudeDegrees / 2;

            var left = center.Longitude - halfwidthDegrees;
            var right = center.Longitude + halfwidthDegrees;
            var top = center.Latitude + halfheightDegrees;
            var bottom = center.Latitude - halfheightDegrees;

            // Adjust for Internation Date Line (+/- 180 degrees longitude)
            if (left < -180) left = 180 + (180 + left);
            if (right > 180) right = (right - 180) - 180;
            // I don't wrap around north or south; I don't think the map control allows this anyway

            Debug.WriteLine("Bounding box:");
            Debug.WriteLine("                    " + top);
            Debug.WriteLine("  " + left + "                " + right);
            Debug.WriteLine("                    " + bottom);
        }
    }
}