﻿using BikeSharing.MobileApp.Pages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace BikeSharing.MobileApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new StartPage();
            MainPage = new NavigationPage(new StartPage() { Title = "BIKE SHARING SAMPLE 0.1" });
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
