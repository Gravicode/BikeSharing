using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BikeSharing.MobileApp.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HelpPage : CarouselPage
	{
		public HelpPage ()
		{
			InitializeComponent ();
            //var image = new Image { Source = "help1.jpg" };
        }
	}
}