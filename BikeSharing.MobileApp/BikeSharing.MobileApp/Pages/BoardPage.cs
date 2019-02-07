using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xam.Plugin.SimpleAppIntro;
using Xamarin.Forms;

namespace BikeSharing.MobileApp.Pages
{
	public class BoardPage : ContentPage
	{
		public BoardPage ()
		{
            Show();
		}
        private void Show()
        {
            var welcomePage = new SimpleAppIntro(new List<object>() {
            new Slide(new SlideConfig("Locate", "Locate bike around the city. See bike availablity on the BikeSharing app.", "locate.png",
            "#437a42", "#ffffff", "#ffffff",
            FontAttributes.Bold, FontAttributes.Italic, 24, 16)),
            new Slide(new SlideConfig("Unlock", "Use BikeSharing app to scan the QR Code and unlock the bike.", "unlock.png",
            "#437a42", "#ffffff", "#ffffff",
            FontAttributes.Bold, FontAttributes.Italic, 24, 16)),
            new Slide(new SlideConfig("Ride", "Enjoy your ride. And when you're done, just park legally and lock the bike to stop rent", "bicycle.png",
            "#437a42", "#ffffff", "#ffffff",
            FontAttributes.Bold, FontAttributes.Italic, 24, 16)),
      })
            {
                // Properties
                ShowPositionIndicator = true,
                ShowSkipButton = true,
                ShowNextButton = true,
                DoneText = "Finish",
                NextText = "Next",
                SkipText = "Skip",
                SkipButtonTextColor = "#437a42",
                NextButtonTextColor = "#437a42",

                // Theming
                BarColor = "#607D8B",
                SkipButtonBackgroundColor = "#ffffff",
                DoneButtonBackgroundColor = "#8AC149",
                NextButtonBackgroundColor = "#ffffff",

                //// Use images instead of buttons
                DoneButtonImage = "baseline_done_white_24.png",
                //NextButtonImage = "baseline_done_white_24.png",

                // Callbacks
                OnSkipButtonClicked = OnSkipButtonClicked,
                OnDoneButtonClicked = OnDoneButtonClicked
            };

            Navigation.PushModalAsync(welcomePage);
        }

        private async void OnSkipButtonClicked()
        {
            //DisplayAlert("Result", "Skip", "OK");
            var tab = new TabbedMenu();
            await Navigation.PushAsync(tab);
        }

        /// <summary>
        /// On done button clicked
        /// </summary>
        private void OnDoneButtonClicked()
        {
            DisplayAlert("Result", "Skip", "OK");
        }
    }
}