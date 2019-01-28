using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
//using BikeSharing.MobileApp.Models;

namespace BikeSharing.MobileApp.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HistoryPage : ContentPage
	{
        //public ObservableCollection<HistoryViewModel> history { get; set; }
        public HistoryPage()
        {
            InitializeComponent();

            //history = new ObservableCollection<HistoryViewModel>();
            //history.Add(new HistoryViewModel { Name = "Tomato", Type = "Fruit", Name2 = "Tomato2", Type2 = "Fruit2", Image = "tomato.png" });
            //history.Add(new HistoryViewModel { Name = "Romaine Lettuce", Type = "Vegetable", Name2 = "Tomato2", Type2 = "Fruit2", Image = "lettuce.png" });
            //history.Add(new HistoryViewModel { Name = "Zucchini", Type = "Vegetable", Name2 = "Tomato2", Type2 = "Fruit2", Image = "zucchini.png" });
            //lstView.ItemsSource = history;
        }
    }
}