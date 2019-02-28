using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BikeSharing.MobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SwitchSlideView : ContentView
	{
		public SwitchSlideView ()
		{
			InitializeComponent ();

            SizeChanged += (sender, args) =>
            {
                string visualState = Width > Height ? "Landscape" : "Portrait";
                VisualStateManager.GoToState(mainStack, visualState);
                VisualStateManager.GoToState(mainGrid, visualState);
                foreach (View child in mainGrid.Children)
                    VisualStateManager.GoToState(child, visualState);
            };
        }
	}
}