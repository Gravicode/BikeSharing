using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BikeSharing.Models;

namespace BikeSharing.Service.Helpers
{
    public class BikeSharingDB : DbContext
    {
        public BikeSharingDB(DbContextOptions<BikeSharingDB> options) : base(options)
        { }

        public DbSet <AppLog> AppLogs { get; set; }
        public DbSet <SoSReport> SoSReports { get; set; }
        public DbSet <SosReportDetail> SosReportDetails { get; set; }
        public DbSet <UserProfile> UserProfiles { get; set; }
        public DbSet <GeoFenceData> GeoFenceDatas { get; set; }
        //public DbSet <GeoFenceDataDetail> FenceDataDetails { get; set; }
        public DbSet <Bike> Bikes { get; set; }
        //public DbSet <BikeCondition> BikeConditions { get; set; }
        public DbSet <BikeStation> BikeStations { get; set; }
        public DbSet <TripHeader> TripHeaders { get; set; }
        public DbSet <TripDetail> TripDetails { get; set; }
        //public DbSet <Location> Locations { get; set; }
        //public DbSet <GpsPoint> GpsPoints { get; set; }
        //public DbSet <DeviceAction> DeviceActions { get; set; }
        //public DbSet <DeviceData> DeviceDatas { get; set; }
        //public DbSet <Trips> Tripss { get; set; }
        //public DbSet <TripInfo> TripInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        private void updateUpdatedProperty<T>() where T : class
        {
            
        }
    }
}
