using System;
using System.Collections.Generic;
using System.Text;
using Xam.Plugin.SimpleAppIntro;
using Xamarin.Forms;

namespace BikeSharing.MobileApp.Pages
{
    public class TabbedMenu : TabbedPage
    {
        public TabbedMenu()
        {
            showboarding();
            this.BarBackgroundColor = Color.FromHex("#748e59");
            this.BarTextColor = Color.White;
            //var navigationPage = new NavigationPage(new LoginPage());
            //navigationPage.Icon = "schedule.png";
            //navigationPage.Title = "Schedule";
            Children.Add(new LoginPage());
            //Children.Add(navigationPage);
            Children.Add(new RegisterPage());
        }

        async void showboarding()
        {
            var page = new BoardingPage();
            await Navigation.PushModalAsync(page);
        }
    }
}
