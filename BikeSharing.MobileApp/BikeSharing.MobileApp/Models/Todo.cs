using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BikeSharing.MobileApp.Models
{
    public class Todo : INotifyPropertyChanged
    {
        //private int _id;
        //[PrimaryKey, AutoIncrement]
        //public int Id
        //{
        //    get { return _id; }
        //    set { _id = value; OnPropertyChanged(); }
        //}

        private string _Title;

        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        private string _Description;

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
