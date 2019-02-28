using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xam.Plugin.SimpleAppIntro;
using Xamarin.Forms;

namespace BikeSharing.MobileApp.Pages
{
    public class HelpPage : ContentPage
    {
        
        public HelpPage()
        {
            var welcomePage = new SimpleAppIntro(new List<object>() {
            new Slide(new SlideConfig("Pertama", "Temukan BikeSharing Anda", "locate.png",
            "#437a42", "#f7ea00", "#ffffff",
            FontAttributes.Bold, FontAttributes.Italic, 24, 16)),
            new Slide(new SlideConfig("Kedua", "Pindai kode QR pada perangkat yang terpasang di BikeSharing untuk membukanya. Note : Jangan lupa untuk mengizinkan aplikasi BikeSharing Kamera!", "unlock.png",
            "#437a42", "#f7ea00", "#ffffff",
            FontAttributes.Bold, FontAttributes.Italic, 24, 16)),
            new Slide(new SlideConfig("Ketiga", "BikeSharing anda sekarang siap untuk dikendarai ke tujuan pilihan Anda! Ingat, selalu bersepeda dengan aman", "bicycle.png",
            "#437a42", "#f7ea00", "#ffffff",
            FontAttributes.Bold, FontAttributes.Italic, 24, 16)),
            new Slide(new SlideConfig("Keempat", "Setelah anda tiba di tujuan anda, parkirlah BikeSharing anda di parkir terdekat untuk mengakhiri perjalanan anda. Sepeda anda akan terkunci secara otomatis.", "bicycle.png",
            "#437a42", "#f7ea00", "#ffffff",
            FontAttributes.Bold, FontAttributes.Italic, 24, 16)),
      })
            {
                // Properties
                ShowPositionIndicator = true,
                ShowSkipButton = true,
                ShowNextButton = true,
                DoneText = "Finish",
                NextText = "Next",
                SkipText = "Back",
                SkipButtonTextColor = "#ffffff",
                NextButtonTextColor = "#ffffff",

                // Theming
                BarColor = "#607D8B",
                SkipButtonBackgroundColor = "#437a42",
                DoneButtonBackgroundColor = "#8AC149",
                NextButtonBackgroundColor = "#437a42",

                //// Use images instead of buttons
                DoneButtonImage = "baseline_done_white_24.png",
                //NextButtonImage = "baseline_done_white_24.png",

                // Callbacks
                OnSkipButtonClicked = OnSkipButtonClicked,
                OnDoneButtonClicked = OnDoneButtonClicked
            };   
        }
        

        private async void OnSkipButtonClicked()
        {
            //DisplayAlert("Result", "Skip", "OK");
            var tab = new RootPage();
           await Navigation.PushAsync(tab);
        }

        /// <summary>
        /// On done button clicked
        /// </summary>
        private async void OnDoneButtonClicked()
        {
            var tab = new RootPage();
            await Navigation.PushModalAsync(tab);
        }
    }
}
