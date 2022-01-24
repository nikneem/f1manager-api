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
        public F1ManagerDbContext() 
        {
            
        }

        public DbSet<DriverPointEntity> DriverPoints { get; set; }
        public DbSet<EnginePointEntity> EnginePoints { get; set; }
        public DbSet<ChassisPointEntity> ChassisPoints { get; set; }

        public DbSet<CircuitEntity> Circuits { get; set; }
        public DbSet<RaceWeekendEntity> RaceWeekends { get; set; }

        public DbSet<RaceResultEntity> RaceResults { get; set; }
        public DbSet<RaceDriverResultsEntity> RaceDriverResults { get; set; }

        public DbSet<TeamEntity> Teams { get; set; }
        public DbSet<TeamEngineEntity> TeamEngines { get; set; }
        public DbSet<TeamChassisEntity> TeamChassis { get; set; }
        public DbSet<TeamDriverEntity> TeamDrivers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}