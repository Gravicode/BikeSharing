using BikeSharing.MobileApp.Helpers;
using BikeSharing.MobileApp.Models;
using BikeSharing.MobileApp.Pages;
using BikeSharing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace BikeSharing.MobileApp
{
    public partial class App : Application
    {
        //public static TodoItemManager TodoManager { get; private set; }
        public static double ScreenHeight;
        public static double ScreenWidth;
        //public static bool IsUserLoggedIn { get; set; }
        public App()
        {
            //if (!IsUserLoggedIn)
            //{
            //    MainPage = new TabbedMenu();
            //}
            //else
            //{
            //    MainPage = new BikeSharing.MobileApp.MainPage();
            //}
            InitializeComponent();
            //MainPage = new StartPage();
            //MainPage = new NavigationPage(new StartPage() { Title = "BIKE SHARING SAMPLE 0.1" });
            //DependencyService.Register<RestEngine<UserProfile>>();
            //TodoManager = new TodoItemManager(new RestService());
            MainPage = new TabbedMenu();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
