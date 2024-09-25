using Go.Core.Entities.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Go.Infrastructure._Data.Configurations.ProductConfigurations
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(P => P.Name)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(P => P.Description)
                   .IsRequired();

            builder.Property(P => P.Price)
                   .HasColumnType("decimal(18,2)");

            //Relation Btw Product => Brand (Many To One)
            builder.HasOne(P => P.Brand)
                   .WithMany()
                   .HasForeignKey(P => P.BrandId);

            //Relation Btw Product => Category (Many To One)
            builder.HasOne(P => P.Category)
                   .WithMany()
                   .HasForeignKey(P => P.CategoryId);

        }
    }
}
