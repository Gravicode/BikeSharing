using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeSharing.Models
{
    public class Location
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        
    }
    public class GpsPoint:Location
    {
        public DateTime Timestamp { get; set; }
        public double SpeedInKnots { get; set; }
        public double BearingInDegrees { get; set; }
    }

    public class DeviceAction
    {
        public string DeviceName { get; set; }
        public string ActionName { get; set; }
        public string Params0 { get; set; }
        public string Params1 { get; set; }
        public string Params2 { get; set; }
        public string Params3 { get; set; }
    }
    public class DeviceData
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public DeviceData(double latitude,double longitude)
        {
            this.Position = new GpsPoint();
            this.Position.Latitude = latitude;
            this.Position.Longitude = longitude;
        }
        public bool IsLocked { get; set; }
        public DateTime TimeStamp { get; set; }
        public GpsPoint Position { get; set; }
        public bool SOS { get; set; }
        public TripInfo Info { get; set; }
    }

    public class SoSReport
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string DeviceName { get; set; }
        public string UserName { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Message { get; set; }
        public GpsPoint Position { get; set; }
    }
    public class Trips
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public TripInfo Info { get; set; }
        public List<GpsPoint> Routes { get; set; }
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
    public class UserProfile:AuditAttribute
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Password { set; get; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfilePictureUrl { get; set; }
        public bool IsActive { get; set; }
    }
    

    public class GeoFenceData:AuditAttribute
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long Id { get; set; }
        public string Name { get; set; }
        public List<Location> Points { get; set; }
    }
}
