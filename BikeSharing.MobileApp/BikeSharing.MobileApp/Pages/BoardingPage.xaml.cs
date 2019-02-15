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
    public partial class BoardingPage : CarouselPage
    {
        public BoardingPage()
        {
            InitializeComponent();

            btnSkip1.Clicked += async (object sender, EventArgs e) =>
            {
                await Navigation.PopModalAsync();
            };
            btnSkip2.Clicked += async (object sender, EventArgs e) =>
            {
                await Navigation.PopModalAsync();
            };
            btnSkip3.Clicked += async (object sender, EventArgs e) =>
            {
                await Navigation.PopModalAsync();
            };
            btnSkip4.Clicked += async (object sender, EventArgs e) =>
            {
                await Navigation.PopModalAsync();
            };
        }
    }
}