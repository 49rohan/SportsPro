using Microsoft.EntityFrameworkCore;
using System;

namespace SportsPro.Models
{
    public class SportsProContext : IdentityDbContext<User>
    {
        public SportsProContext(DbContextOptions<SportsProContext> options)
            : base(options)
        {
        }

        // Add DbSet properties for each entity
        public DbSet<Product> Products { get; set; }
        public DbSet<Technician> Technicians { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<Registration> Registrations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Your model configurations
            modelBuilder.Entity<Registration>()
                .HasKey(r => r.RegistrationId);

            modelBuilder.Entity<Registration>()
                .HasOne(r => r.Customer)
                .WithMany(c => c.Registrations)
                .HasForeignKey(r => r.CustomerID);

            modelBuilder.Entity<Registration>()
                .HasOne(r => r.Product)
                .WithMany(p => p.Registrations)
                .HasForeignKey(r => r.ProductID);

            // Apply configuration classes (which include HasData)
            modelBuilder.ApplyConfiguration(new ProductConfig());
            modelBuilder.ApplyConfiguration(new TechnicianConfig());
            modelBuilder.ApplyConfiguration(new CountryConfig());
            modelBuilder.ApplyConfiguration(new CustomerConfig());
            modelBuilder.ApplyConfiguration(new IncidentConfig());
        }
    }
}
