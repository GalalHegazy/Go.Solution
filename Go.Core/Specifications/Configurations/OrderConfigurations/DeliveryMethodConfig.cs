using Go.Core.Entities.Order_Aggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Go.Infrastructure._Data.Configurations.OrderConfigurations
{
    internal class DeliveryMethodConfig : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(DeliveryMethod => DeliveryMethod.Cost)
                 .HasColumnType("decimal(12,2)");
        }
    }
}
