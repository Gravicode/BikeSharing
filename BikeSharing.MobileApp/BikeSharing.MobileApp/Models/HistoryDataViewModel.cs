using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using BikeSharing.MobileApp.Models;

namespace BikeSharing.MobileApp.Models
{
    public class HistoryDataViewModel
    {
        public IList<HistoryDataModel> HistoryDataCollection { get; set; }

        public object SelectedItem { get; set; }

        public HistoryDataViewModel()
        {
            HistoryDataCollection = new List<HistoryDataModel>();
            GenerateCardModel();
        }

        private void GenerateCardModel()
        {
            for (var i = 0; i < 10; i++)
            {
                var cardData = new HistoryDataModel()
                {
                    Title = i + "KM",
                    Owner = "BikeSharing" + i,
                    AlertColor = i % 2 == 0 ? Color.ForestGreen : Color.ForestGreen,
                };
                HistoryDataCollection.Add(cardData);
            }
        }
    }
}
