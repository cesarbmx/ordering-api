using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CesarBmx.Ordering.Domain.Models;
using Microsoft.EntityFrameworkCore;
using CesarBmx.Shared.Persistence.Extensions;

namespace CesarBmx.Ordering.Persistence.Mappings
{
    public static class OrderMapping
    {
        public static void Map(this EntityTypeBuilder<Order> entityBuilder)
        {
            // Key
            entityBuilder.HasKey(t => t.OrderId);

            // Properties
            entityBuilder.Property(t => t.OrderId)
                .HasColumnType("int")
                .IsRequired()
                .ValueGeneratedOnAdd();

            entityBuilder.Property(t => t.UserId)
                .HasColumnType("nvarchar(50)")
                .HasMaxLength(50)
                .IsRequired();

            entityBuilder.Property(t => t.CurrencyId)
                .HasColumnType("nvarchar(50)")
                .HasMaxLength(50)
                .IsRequired();

            entityBuilder.Property(t => t.Price)
                .HasColumnType("decimal(18,4)")
                .IsRequired();

            entityBuilder.Property(t => t.Quantity)
                .HasColumnType("decimal(18,4)")
                .IsRequired();

            entityBuilder.Property(t => t.OrderType)
                .HasColumnType("varchar(50)")
                .HasMaxLength(50)
                .HasStringToEnumConversion()
                .IsRequired();

            entityBuilder.Property(t => t.OrderStatus)
                .HasColumnType("varchar(50)")
                .HasMaxLength(50)
                .HasStringToEnumConversion()
                .IsRequired();

            entityBuilder.Property(t => t.PlacedAt)
                .HasColumnType("datetime2");

            entityBuilder.Property(t => t.FilledAt)
                .HasColumnType("datetime2");
        }
    }
}
