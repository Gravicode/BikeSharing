using System;
using System.Collections.Generic;

namespace BikeSharing.Models
{
    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class DeviceAction
    {
        public string ActionName { get; set; }
        public string[] Params { get; set; }
    }
   public class DeviceData
    {
        public DeviceData(double latitude,double longitude)
        {
            this.Position = new Location();
            this.Position.Latitude = latitude;
            this.Position.Longitude = longitude;
        }
        public DateTime TimeStamp { get; set; }
        public Location Position { get; set; }
        public bool SOS { get; set; }
        public TripInfo Info { get; set; }
    }

    public class SoSReport
    {
        public string DeviceName { get; set; }
        public string UserName { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Message { get; set; }
        public Location Position { get; set; }
    }
    public class Trips
    {
        public TripInfo Info { get; set; }
        public List<Route> Routes { get; set; }
    }

    public class TripInfo
    {
        public string UserName { get; set; }
        public string TripNumber { get; set; }
        public string DeviceName { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    public class UserProfile
    {
        public string UserName { get; set; }
        public string Password { set; get; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class Route
    {
        public Location Position { get; set; }
        public DateTime TimeStamp { get; set; }
    }

    public class GeoFenceData
    {
        public int LocationID { get; set; }
        public string Name { get; set; }
        public List<Location> Points { get; set; }
    }
}
