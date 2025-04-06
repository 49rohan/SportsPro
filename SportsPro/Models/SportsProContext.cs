using System;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models.Configurations;

namespace SportsPro.Models
{
    public class SportsProContext : DbContext
    {
        public SportsProContext(DbContextOptions<SportsProContext> options)
            : base(options)
        { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Technician> Technicians { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Incident> Incidents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductConfig());
            modelBuilder.ApplyConfiguration(new TechnicianConfig());
            modelBuilder.ApplyConfiguration(new CountryConfig());
            modelBuilder.ApplyConfiguration(new CustomerConfig());
            modelBuilder.ApplyConfiguration(new IncidentConfig());
        }
    }
}