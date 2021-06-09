using F1Manager.SqlData.Entities;
using Microsoft.EntityFrameworkCore;

namespace F1Manager.SqlData
{
    public class F1ManagerDbContext : DbContext
    {
        public F1ManagerDbContext(DbContextOptions options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<CircuitEntity> Circuits { get; set; }

        public DbSet<TeamEntity> Teams { get; set; }
        public DbSet<TeamEngineEntity> TeamEngines { get; set; }
        public DbSet<TeamChassisEntity> TeamChassis { get; set; }
        public DbSet<TeamDriverEntity> TeamDrivers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}