using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;


namespace BikeSharing.MobileApp.Pages
{
    public class RootPage : MasterDetailPage
    {
        MasterPage masterPage;

        public RootPage()
        {
            ShowLoginDialog();
            masterPage = new MasterPage();
            Master = masterPage;
            Detail = new NavigationPage(new MapPage());

            masterPage.ListView.ItemSelected += OnItemSelected;

            if (Device.RuntimePlatform == Device.UWP)
            {
                MasterBehavior = MasterBehavior.Popover;
            }
        }

        async void ShowLoginDialog()
        {
            var page = new TabbedMenu();
            await Navigation.PushModalAsync(page);
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                masterPage.ListView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}
