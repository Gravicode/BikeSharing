using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeSharing.Models
{
    #region NOSQL
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
        public DeviceData()
        {
            this.Position = new GpsPoint();
        }
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
    #endregion

    #region SQL
    public enum LogTypes { Error, Info, Other };
    public class AppLog
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string LogID { get; set; }
        public DateTime LogDate { get; set; }
        public LogTypes LogType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }

    }
    public enum SoSReportType { Reported, InProgress, Responded}
    public class SoSReport
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string DeviceName { get; set; }
        public string UserName { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Message { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public SoSReportType Status { get; set; }
    }

    public class SosReportDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public string AttachmentUrl { get; set; }
        public string ProfPic { get; set; }
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
       
    }

    public class GeoFenceDataDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long GeoFenceDataId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

    }

    public enum BikeCondition
    {
        Active, Missing, Broken, Maintenance
    }
    public class Bike
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string BikeNo { get; set; }
        public long StationId { get; set; }
        public BikeCondition Condition { get; set; }
    }
    public class BikeStation:AuditAttribute
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public int TotalBike { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Name { get; set; }
        public int BikeUsed { get; set; }
        public int BikeBroken { get; set; }
        public int BikeAvailable { get; set; }
    }
    public class TripHeader
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string UserName { get; set; }
        public string TripNumber { get; set; }
        public string BikeNo { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan Duration { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double TotalDistanceMeter { get; set; }
    }
    public class TripDetail:GpsPoint
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long TripHeaderId { get; set; }

        public string TripNumber { set; get; }

    }   
    #endregion
}
