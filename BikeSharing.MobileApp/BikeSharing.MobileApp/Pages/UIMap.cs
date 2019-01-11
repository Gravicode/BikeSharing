using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Diagnostics;
using Plugin.Geolocator;
using System.Collections.ObjectModel;
using BikeSharing.MobileApp.Pages;

namespace BikeSharing
{
    public class UIMap : ContentPage
    {
        Map map;
        public UIMap()
        {
            map = new Map
            {
                //IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            // You can use MapSpan.FromCenterAndRadius 
            //map.MoveToRegion (MapSpan.FromCenterAndRadius (new Position (37, -122), Distance.FromMiles (0.3)));

            //map.MoveToRegion(new MapSpan(new Position(0, 0), 360, 360));

            var segments = new StackLayout
            {
                Spacing = 30,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Orientation = StackOrientation.Horizontal,
                //Children = { street, hybrid, satellite }
            };


            // put the page together
            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(map);
            //stack.Children.Add(slider);
            stack.Children.Add(segments);
            Content = stack;


            // for debugging output only
            map.PropertyChanged += (sender, e) => {
                Debug.WriteLine(e.PropertyName + " just changed!");
                if (e.PropertyName == "VisibleRegion" && map.VisibleRegion != null)
                    CalculateBoundingCoordinates(map.VisibleRegion);
            };
        }
        

        async void currentloc()
        {
            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude),
                                                         Distance.FromMiles(1)));
        }

        void HandleClicked(object sender, EventArgs e)
        {
            var b = sender as Button;
            switch (b.Text)
            {
                case "Street":
                    map.MapType = MapType.Street;
                    break;
                case "Hybrid":
                    map.MapType = MapType.Hybrid;
                    break;
                case "Satellite":
                    map.MapType = MapType.Satellite;
                    break;
            }
        }


        /// <summary>
        /// In response to this forum question http://forums.xamarin.com/discussion/22493/maps-visibleregion-bounds
        /// Useful if you need to send the bounds to a web service or otherwise calculate what
        /// pins might need to be drawn inside the currently visible viewport.
        /// </summary>
        static async void CalculateBoundingCoordinates(MapSpan region)
        {

            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
            region = (MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude),
                                                         Distance.FromMiles(1)));
            // WARNING: I haven't tested the correctness of this exhaustively!
            //var center = region.Center;
            //var halfheightDegrees = region.LatitudeDegrees / 2;
            //var halfwidthDegrees = region.LongitudeDegrees / 2;

            //var left = center.Longitude - halfwidthDegrees;
            //var right = center.Longitude + halfwidthDegrees;
            //var top = center.Latitude + halfheightDegrees;
            //var bottom = center.Latitude - halfheightDegrees;

            // Adjust for Internation Date Line (+/- 180 degrees longitude)
            //if (left < -180) left = 180 + (180 + left);
            //if (right > 180) right = (right - 180) - 180;
            // I don't wrap around north or south; I don't think the map control allows this anyway

            //Debug.WriteLine("Bounding box:");
            //Debug.WriteLine("                    " + top);
            //Debug.WriteLine("  " + left + "                " + right);
            //Debug.WriteLine("                    " + bottom);
        }
    }
}
