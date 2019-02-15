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
                await Navigation.PushModalAsync(new RootPage());
            }
		}
	}
}