using Go.Core.Entities.Order_Aggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net.NetworkInformation;

namespace Go.Infrastructure._Data.Configurations.OrderConfig
{
    internal class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            //  To How to Map composite attribute (ShippingAddress) in Table (Order)
            builder.OwnsOne(Order => Order.Address,Address=>Address.WithOwner());

            // OrderStatus 
            builder.Property(Order => Order.Status).HasConversion(
                (OStatus) => OStatus.ToString(),
                (OStatus) => (OrderStatus) Enum.Parse(typeof(OrderStatus), OStatus)
                );

            builder.Property(Order => Order.SubTotal)
                   .HasColumnType("decimal(12,2)");

            // set Realation (1) by FluntApi  in Table Order (One) form DeliveryMethod and Many From Order ,and Change on delete behaivor form Cascade to set null.
            builder.HasOne(Order => Order.DeliveryMethod)
                   .WithMany()
                   .OnDelete(DeleteBehavior.SetNull);

            // set Realation (2) by FluntApi  in Table Order (One) form Order and Many From Items  ,and Change on delete behaivor form noAction to set Cascade.
            builder.HasMany(Order => Order.Items)
                   .WithOne()   
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
