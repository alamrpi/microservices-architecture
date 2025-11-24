using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Enums;
using Ordering.Domain.Models;

namespace Ordering.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        // 1. Table Name
        builder.ToTable("Orders");

        // 2. Primary Key
        builder.HasKey(o => o.Id);

        builder.Ignore(o => o.TotalPrice);

        builder.Property(o => o.OrderName)
            .HasMaxLength(100)
            .IsRequired();

        // 4. Value Object Mapping (Address) - Complex Mapping
        builder.OwnsOne(o => o.ShippingAddress, shippingAddress =>
        {
            shippingAddress.Property(a => a.FirstName).HasMaxLength(50).HasColumnName("Shipping_FirstName");
            shippingAddress.Property(a => a.LastName).HasMaxLength(50).HasColumnName("Shipping_LastName");
            shippingAddress.Property(a => a.EmailAddress).HasMaxLength(50);
            shippingAddress.Property(a => a.AddressLine).HasMaxLength(180);
            shippingAddress.Property(a => a.Country).HasMaxLength(50);
            shippingAddress.Property(a => a.State).HasMaxLength(50);
            shippingAddress.Property(a => a.ZipCode).HasMaxLength(10);
        });

        builder.OwnsOne(o => o.BillingAddress, billingAddress =>
        {
            billingAddress.Property(a => a.FirstName).HasMaxLength(50).HasColumnName("Billing_FirstName");
            billingAddress.Property(a => a.LastName).HasMaxLength(50).HasColumnName("Billing_LastName");
            billingAddress.Property(a => a.EmailAddress).HasMaxLength(50).HasColumnName("Billing_Email");
            billingAddress.Property(a => a.AddressLine).HasMaxLength(180).HasColumnName("Billing_AddressLine");
            billingAddress.Property(a => a.Country).HasMaxLength(50).HasColumnName("Billing_Country");
            billingAddress.Property(a => a.State).HasMaxLength(50).HasColumnName("Billing_State");
            billingAddress.Property(a => a.ZipCode).HasMaxLength(10).HasColumnName("Billing_ZipCode");
        });

        builder.OwnsOne(o => o.Payment, payment =>
        {
            payment.Property(p => p.CardName).HasMaxLength(50);
            payment.Property(p => p.CardNumber).HasMaxLength(24);
            payment.Property(p => p.Expiration).HasMaxLength(5);
            payment.Property(p => p.CVV).HasMaxLength(3);
            payment.Property(p => p.PaymentMethod);
        });

        // 5. Enum Conversion
        builder.Property(o => o.Status)
            .HasConversion(
                s => s.ToString(),
                dbStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), dbStatus));

        // 6. Relationship (Order -> OrderItems)
        builder.HasMany(o => o.OrderItems)
               .WithOne()
               .HasForeignKey(oi => oi.OrderId);
    }
}