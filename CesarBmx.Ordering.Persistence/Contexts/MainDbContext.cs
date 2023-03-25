using System.Diagnostics.CodeAnalysis;
using CesarBmx.Ordering.Domain.Models;
using CesarBmx.Ordering.Persistence.Mappings;
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

        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().Map();

            base.OnModelCreating(modelBuilder);
        }
    }
}
