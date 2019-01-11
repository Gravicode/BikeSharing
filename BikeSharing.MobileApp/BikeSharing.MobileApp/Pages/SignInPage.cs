using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BikeSharing.MobileApp.Pages
{
    public class SignInPage : ContentPage
    {
        Label messageLabel;
        public SignInPage()
        {
            //Icon = "settings.png";
            Title = "LOGIN";
            //Content = new StackLayout
            //{
            //    Children = {
            //        new Label {
            //            Text = "Test Sign In",
            //            HorizontalOptions = LayoutOptions.Center,
            //            VerticalOptions = LayoutOptions.CenterAndExpand
            //        }
            //    }
            //};
            
            //BindingContext = new LoginViewModel(Navigation);

            //BackgroundColor = Helpers.Color.Purple.ToFormsColor();

            var layout = new StackLayout { Padding = 10 };

            var label = new Label
            {
                Text = "Welcome To BikeSharing",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalTextAlignment = TextAlignment.Center, // Center the text in the blue box.
                VerticalTextAlignment = TextAlignment.Center, // Center the text in the blue box.
            };

            layout.Children.Add(label);

            var username = new Entry { Placeholder = "Username" };
            //username.SetBinding(Entry.TextProperty, LoginViewModel.UsernamePropertyName);
            layout.Children.Add(username);

            var password = new Entry { Placeholder = "Password", IsPassword = true };
            //password.SetBinding(Entry.TextProperty, LoginViewModel.PasswordPropertyName);
            layout.Children.Add(password);

            var button = new Button { Text = "Sign In", TextColor = Color.Blue };
            //button.SetBinding(Button.CommandProperty, LoginViewModel.LoginCommandPropertyName);
            button.Clicked += button_Clicked;
            async void button_Clicked(object sender, EventArgs e)
            {
                await Navigation.PopModalAsync();
                //var user = new User
                //{
                //    Username = username.Text,
                //    Password = password.Text
                //};

                //var isValid = AreCredentialsCorrect(user);
                //if (isValid)
                //{   
                //    App.IsUserLoggedIn = true;
                //    Navigation.InsertPageBefore(new RootPage(), this);
                //    await Navigation.PopAsync();
                //}
                //else
                //{
                //    messageLabel.Text = "Login failed";
                //    password.Text = string.Empty;
                //}
            }

            //bool AreCredentialsCorrect(User user)
            //{
            //    return user.Username == Constants.Username && user.Password == Constants.Password;
            //}

            layout.Children.Add(button);

            var button2 = new Button { Text = "Sign In with Facebook", TextColor = Color.White, BackgroundColor = Color.Blue };
            //button.SetBinding(Button.CommandProperty, LoginViewModel.LoginCommandPropertyName);

            layout.Children.Add(button2);
            Content = new ScrollView { Content = layout };
        }
    }
}
