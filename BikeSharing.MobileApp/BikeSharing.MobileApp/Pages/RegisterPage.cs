using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BikeSharing.MobileApp.Pages
{
    public class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            //Icon = "settings.png";
            Title = "Register";
            //BindingContext = new LoginViewModel(Navigation);

            //BackgroundColor = Helpers.Color.Purple.ToFormsColor();

            var layout = new StackLayout { Padding = 10 };

            var firstname = new Entry { Placeholder = "First Name" };
            //username.SetBinding(Entry.TextProperty, LoginViewModel.UsernamePropertyName);
            layout.Children.Add(firstname);

            var lastname = new Entry { Placeholder = "Last Name" };
            //password.SetBinding(Entry.TextProperty, LoginViewModel.PasswordPropertyName);
            layout.Children.Add(lastname);

            var username = new Entry { Placeholder = "Username" };
            //username.SetBinding(Entry.TextProperty, LoginViewModel.UsernamePropertyName);
            layout.Children.Add(username);

            var password = new Entry { Placeholder = "Password", IsPassword = true };
            //password.SetBinding(Entry.TextProperty, LoginViewModel.PasswordPropertyName);
            layout.Children.Add(password);


            var button = new Button { Text = "Register", TextColor = Color.White };
            //button.SetBinding(Button.CommandProperty, LoginViewModel.LoginCommandPropertyName);

            layout.Children.Add(button);

            var button2 = new Button { Text = "Register with Facebook", TextColor = Color.White, BackgroundColor = Color.Blue };
            //button.SetBinding(Button.CommandProperty, LoginViewModel.LoginCommandPropertyName);

            layout.Children.Add(button2);
            Content = new ScrollView { Content = layout };
        }
    }
}
