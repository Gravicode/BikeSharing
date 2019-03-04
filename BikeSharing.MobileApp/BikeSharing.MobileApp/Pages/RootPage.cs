using System;
using System.Collections.Generic;
using System.Text;
using Xam.Plugin.SimpleAppIntro;
using Xamarin.Forms;


namespace BikeSharing.MobileApp.Pages
{
    public class RootPage : MasterDetailPage
    {
        MasterPage masterPage;

        public RootPage()
        {
            masterPage = new MasterPage();
            Master = masterPage;
            Detail = new NavigationPage(new StartPage());

            masterPage.ListView.ItemSelected += OnItemSelected;

            if (Device.RuntimePlatform == Device.UWP)
            {
                MasterBehavior = MasterBehavior.Popover;
            }
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
