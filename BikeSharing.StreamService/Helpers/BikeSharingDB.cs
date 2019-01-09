using BikeSharing.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BikeSharing.StreamService.Helpers
{
    public class BikeSharingDB : DbContext
    {

        public BikeSharingDB(string connString) : base(GetOptions(connString))
        {
           
        }
        private static DbContextOptions GetOptions(string connectionString)
        {
            return MySqlDbContextOptionsExtensions.UseMySql(new DbContextOptionsBuilder(), connectionString).Options;
        }

        public DbSet<AppLog> AppLogs { get; set; }
        public DbSet<SoSReport> SoSReports { get; set; }
        public DbSet<SosReportDetail> SosReportDetails { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<GeoFenceData> GeoFenceDatas { get; set; }
        public DbSet<GeoFenceDataDetail> GeoFenceDataDetails { get; set; }
        public DbSet<Bike> Bikes { get; set; }
        public DbSet<BikeStation> BikeStations { get; set; }
        public DbSet<TripHeader> TripHeaders { get; set; }
        public DbSet<TripDetail> TripDetails { get; set; }
       


        protected override void OnModelCreating(ModelBuilder builder)
        {
            /*
            builder.Entity<DataEventRecord>().HasKey(m => m.DataEventRecordId);
            builder.Entity<SourceInfo>().HasKey(m => m.SourceInfoId);

            // shadow properties
            builder.Entity<DataEventRecord>().Property<DateTime>("UpdatedTimestamp");
            builder.Entity<SourceInfo>().Property<DateTime>("UpdatedTimestamp");
            */
            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            /*
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<SourceInfo>();
            updateUpdatedProperty<DataEventRecord>();
            */
            return base.SaveChanges();
        }

        private void updateUpdatedProperty<T>() where T : class
        {
            /*
            var modifiedSourceInfo =
                ChangeTracker.Entries<T>()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in modifiedSourceInfo)
            {
                entry.Property("UpdatedTimestamp").CurrentValue = DateTime.UtcNow;
            }
            */
        }

    }
}
