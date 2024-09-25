using Go.Core.Entities.Order_Aggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Go.Infrastructure._Data.Configurations.OrderConfigurations
{
    internal class OrderItemsConfig : IEntityTypeConfiguration<OrderItems>
    {
        public void Configure(EntityTypeBuilder<OrderItems> builder)
        {
            //  To How to Map composite attribute (ProductItemOrdered) in Table (OrderItem)
            builder.OwnsOne(OrderItem => OrderItem.Product, Product => Product.WithOwner());

            builder.Property(OrderItem => OrderItem.Price)
                .HasColumnType("decimal(12,2)");
        }
    }
}
