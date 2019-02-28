﻿using BikeSharing.MobileApp.ServicesHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BikeSharing.MobileApp.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			InitializeComponent ();

            btnLogin.Clicked += OnLoginClicked;
            async void OnLoginClicked(object sender, EventArgs e)
            {
                LoginService services = new LoginService();
                var getLoginDetails = await services.CheckLoginIfExists(EntryUsername.Text, EntryPassword.Text);

                if (getLoginDetails)
                {
                    await DisplayAlert("Login success", "You are login", "Okay", "Cancel");
                    await Navigation.PushModalAsync(new RootPage());
                }
                else
                {
                    await DisplayAlert("Login failed", "Username or Password is incorrect or not exists", "Okay", "Cancel");
                }
            }
		}
	}
}