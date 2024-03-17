using Microsoft.EntityFrameworkCore;
using FLEETCORE.Models;

namespace FLEETCORE
{
    public class FLEETCOREDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Refueling> Refuelings { get; set; }
        public DbSet<TimeSheet> TimeSheets { get; set; }
        public DbSet<Leave> Leaves { get; set; }

        private string _connectionString =
            "server=.;database=FLEETCORE;trusted_connection=true;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Vehicle>()
                .HasMany(x => x.Refuelings)
                .WithOne(x => x.Vehicle)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(x => x.Refuelings)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasMany(x => x.TimeSheets)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
