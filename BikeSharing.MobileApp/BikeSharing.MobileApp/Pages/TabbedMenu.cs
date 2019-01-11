using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BikeSharing.MobileApp.Pages
{
    public class TabbedMenu : TabbedPage
    {
        public TabbedMenu()
        {
            //var navigationPage = new NavigationPage(new LoginPage());
            //navigationPage.Icon = "schedule.png";
            //navigationPage.Title = "Schedule";

            Children.Add(new SignInPage());
            //Children.Add(navigationPage);
            Children.Add(new RegisterPage());
        }
    }
}
