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
            Image img = new Image { Source = "Stadion.jpg", VerticalOptions = LayoutOptions.Start };
            Label stationname = new Label { Text = "Lapangan Sempur ", VerticalOptions = LayoutOptions.End };
            Label bikeavailable = new Label { Text = "10", VerticalOptions = LayoutOptions.End, HorizontalOptions = LayoutOptions.End };

            Image img2 = new Image { Source = "Stadion.jpg", VerticalOptions = LayoutOptions.Start, Margin = 5 };
            Label stationname2 = new Label { Text = "Kebun Raya ", VerticalOptions = LayoutOptions.End };
            Label bikeavailable2 = new Label { Text = "5", VerticalOptions = LayoutOptions.End, HorizontalOptions = LayoutOptions.End };

            Image img3 = new Image { Source = "Stadion.jpg", VerticalOptions = LayoutOptions.Start };
            Label stationname3 = new Label { Text = "Botanical Garden ", VerticalOptions = LayoutOptions.End };
            Label bikeavailable3 = new Label { Text = "7", VerticalOptions = LayoutOptions.End, HorizontalOptions = LayoutOptions.End };

            Image img4 = new Image { Source = "Stadion.jpg", VerticalOptions = LayoutOptions.Start };
            Label stationname4 = new Label { Text = "Bogor Permai ", VerticalOptions = LayoutOptions.End };
            Label bikeavailable4 = new Label { Text = "10", VerticalOptions = LayoutOptions.End, HorizontalOptions = LayoutOptions.End };

            Grid asd = new Grid { };
            asd.Children.Add(img);
            asd.Children.Add(stationname);
            asd.Children.Add(bikeavailable);

            Grid asd2 = new Grid { };
            asd2.Children.Add(img2);
            asd2.Children.Add(stationname2);
            asd2.Children.Add(bikeavailable2);

            Grid asd3 = new Grid { };
            asd3.Children.Add(img3);
            asd3.Children.Add(stationname3);
            asd3.Children.Add(bikeavailable3);

            Grid asd4 = new Grid { };
            asd4.Children.Add(img4);
            asd4.Children.Add(stationname4);
            asd4.Children.Add(bikeavailable4);
            try
            {
                Title = "Maps";
                map = new Map
                {
                    IsShowingUser = true,
                    HeightRequest = 500,
                    WidthRequest = 960,
                    VerticalOptions = LayoutOptions.FillAndExpand

                };
                loc();
                async void loc()
                {
                    var locator = CrossGeolocator.Current;
                    var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));

                    map.MoveToRegion(MapSpan.FromCenterAndRadius(
                    new Position(position.Latitude, position.Longitude/*-6.601361, 106.805080*/), Distance.FromMiles(3)).WithZoom(20));
                }



                //async void address()
                //{
                //    //Geocoder gcd = new Geocoder(context, Locale.getDefault());
                //    List<Address> addresses = geocode.GetAddressesForPositionAsync(, lng, 1);
                //    if (addresses.() > 0)
                //    {
                //        System.out.println(addresses.get(0).getLocality());
                //    }
                //    else
                //    {
                //        // do your stuff
                //    }
                //}


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

                var frame = new Frame
                {
                    BorderColor = Color.ForestGreen,
                    HasShadow = true,
                    HeightRequest = 90,
                    WidthRequest = 90,
                    VerticalOptions = LayoutOptions.EndAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Margin = new Thickness(-100, 30, 200, 100),
                    Content = asd
                };
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += (sender, e) =>
                {
                    map.MoveToRegion(MapSpan.FromCenterAndRadius(
                                       new Position(-6.601361, 106.805080), Distance.FromMiles(3)).WithZoom(20));
                };
                frame.GestureRecognizers.Add(tapGestureRecognizer);

                var frame2 = new Frame
                {
                    BorderColor = Color.ForestGreen,
                    HasShadow = true,
                    HeightRequest = 90,
                    WidthRequest = 90,
                    VerticalOptions = LayoutOptions.EndAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Margin = new Thickness(10, 100, 30, 100),
                    Content = asd2
                };
                var tapGestureRecognizer2 = new TapGestureRecognizer();
                tapGestureRecognizer2.Tapped += (sender, e) =>
                {
                    map.MoveToRegion(MapSpan.FromCenterAndRadius(
                                       new Position(-6.592302, 106.800424), Distance.FromMiles(3)).WithZoom(20));
                };
                frame2.GestureRecognizers.Add(tapGestureRecognizer2);

                var frame3 = new Frame
                {
                    BorderColor = Color.ForestGreen,
                    HasShadow = true,
                    HeightRequest = 90,
                    WidthRequest = 90,
                    VerticalOptions = LayoutOptions.EndAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Margin = new Thickness(30, 330, -230, 100),
                    Content = asd3
                };

                var frame4 = new Frame
                {
                    BorderColor = Color.ForestGreen,
                    HasShadow = true,
                    HeightRequest = 90,
                    WidthRequest = 90,
                    VerticalOptions = LayoutOptions.EndAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Margin = new Thickness(30, 30, -510, 100),
                    Content = asd4
                };

                var btnRent = new Button
                {
                    VerticalOptions = LayoutOptions.EndAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Margin = new Thickness(30, 30, 30, 30),
                    BorderRadius = 20,
                    WidthRequest = 180,
                    Text = "Rent Bike",
                    BackgroundColor = Color.ForestGreen,
                    TextColor = Color.White,
                    FontSize = 20
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
                    HorizontalOptions = LayoutOptions.EndAndExpand,
                    Margin = new Thickness(30, 30, 30, 30),
                    BorderRadius = 20,
                    WidthRequest = 100,
                    Text = "SOS",
                    BackgroundColor = Color.Red,
                    TextColor = Color.White,
                    FontSize = 20
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
                var stack2 = new StackLayout();
                stack2.Children.Add(frame);
                stack2.Children.Add(frame2);
                stack2.Children.Add(frame3);
                stack2.Children.Add(frame4);

                var stack = new Grid { };
                stack.Children.Add(map);
                stack.Children.Add(frame);
                stack.Children.Add(frame2);
                stack.Children.Add(frame3);
                stack.Children.Add(frame4);
                stack.Children.Add(btnSos);
                stack.Children.Add(segments);
                stack.Children.Add(btnRent);
                Content = stack;

                //Content = new ScrollView()
                //{
                //    HorizontalOptions = LayoutOptions.FillAndExpand,
                //    Orientation = ScrollOrientation.Horizontal,
                //    Content = stack,
                //};


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