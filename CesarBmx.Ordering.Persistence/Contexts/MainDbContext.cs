using System.Diagnostics.CodeAnalysis;
using CesarBmx.Ordering.Domain.Models;
using CesarBmx.Ordering.Persistence.Mappings;
using CesarBmx.Shared.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CesarBmx.Ordering.Persistence.Contexts
{
    public class MainDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }

        public MainDbContext(DbContextOptions<MainDbContext> options)
           : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().Map();

            base.OnModelCreating(modelBuilder);

            // Masstransit outbox
            modelBuilder.UseMasstransitOutbox();
        }
    }
}
