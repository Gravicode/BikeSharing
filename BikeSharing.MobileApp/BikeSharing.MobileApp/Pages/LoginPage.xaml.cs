﻿using BikeSharing.MobileApp.ServicesHandler;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using BikeSharing;

namespace BikeSharing.MobileApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        String uname, pass;
        public LoginPage()
        {
            InitializeComponent();
            btnLogin.Clicked += OnLoginClicked;
        }
            private async void OnLoginClicked(object sender, EventArgs e)
            {
            LoginService services = new LoginService();
            var getLoginDetails = await services.CheckLoginIfExists(EntryUsername.Text, EntryPassword.Text);

            if (getLoginDetails)
            {
                await DisplayAlert("Login success", "You are login", "Okay", "Cancel");
                await Navigation.PushModalAsync(new UserProfilePage());
            }
            else
            {
                await DisplayAlert("Login failed", "Username or Password is incorrect or not exists", "Okay", "Cancel");
            }
        }

        public async void LoginAsync()
        {
            try
            {
                LoginService services = new LoginService();
                var getLoginDetails = await services.CheckLoginIfExists(EntryUsername.Text, EntryPassword.Text);

                if (getLoginDetails)
                {
                    await DisplayAlert("Login success", "You are login", "Okay", "Cancel");
                    await Navigation.PushModalAsync(new UserProfilePage());
                }
                else
                {
                    await DisplayAlert("Login failed", "Username or Password is incorrect or not exists", "Okay", "Cancel");
                }
            }
            catch (Exception e)
            {
                await DisplayAlert("Alert", e.Message, "Ok");
            }
        }
    }
}