using System.Diagnostics.CodeAnalysis;
using CesarBmx.Notification.Domain.Models;
using CesarBmx.Notification.Persistence.Mappings;
using Microsoft.EntityFrameworkCore;

namespace CesarBmx.Notification.Persistence.Contexts
{
    public class MainDbContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }

        public MainDbContext(DbContextOptions<MainDbContext> options)
           : base(options)
        {
        }

        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>().Map();

            base.OnModelCreating(modelBuilder);
        }
    }
}
