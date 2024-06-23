using Microsoft.EntityFrameworkCore;
using FLEETCORE.Models;

namespace FLEETCORE
{
    public class FLEETCOREDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        private string _connectionString =
            "server=(localdb)\\local;database=FLEETCORE;trusted_connection=true;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {           
            modelBuilder.Entity<Vehicle>()
                .HasOne(x => x.Tag)
                .WithOne(x => x.Vehicle)
                .HasForeignKey<Tag>(t => t.VehicleId)
                .IsRequired(false);

            modelBuilder.Entity<Tag>()
                .HasOne(x => x.Vehicle)
                .WithOne(x => x.Tag)
                .HasForeignKey<Vehicle>(t => t.TagId)
                .IsRequired(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
