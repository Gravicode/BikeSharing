using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BikeSharing.MobileApp.Pages
{
    public class MasterPage : ContentPage
    {
        public ListView ListView { get { return listView; } }

        ListView listView;

        public MasterPage()
        {
            var masterPageItems = new List<MasterPageItem>();
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Profil",
                IconSource = "profile.png",
                TargetType = typeof(UserProfilePage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Perjalanan Saya",
                IconSource = "trip.png",
                TargetType = typeof(StartPage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Maps",
                IconSource = "maps.png",
                TargetType = typeof(MapPage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Bike Sharing",
                IconSource = "bike.jpg",
                TargetType = typeof(StartPage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Bantuan",
                IconSource = "bike.jpg",
                TargetType = typeof(HelpPage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Keluar",
                IconSource = "out.jpg",
                TargetType = typeof(SignInPage)
            });

            listView = new ListView
            {
                ItemsSource = masterPageItems,
                ItemTemplate = new DataTemplate(() =>
                {
                    var grid = new Grid { Padding = new Thickness(5, 10) };
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

                    var image = new Image();
                    image.SetBinding(Image.SourceProperty, "IconSource");
                    var label = new Label { VerticalOptions = LayoutOptions.FillAndExpand };
                    label.SetBinding(Label.TextProperty, "Title");

                    grid.Children.Add(image);
                    grid.Children.Add(label, 1, 0);

                    return new ViewCell { View = grid };
                }),
                SeparatorVisibility = SeparatorVisibility.None
            };

            Icon = "hamburger.png";
            Title = "Personal Organiser";
            Padding = new Thickness(0, 40, 0, 0);
            Content = new StackLayout
            {
                Children = { listView }
            };
        }
    }
}
