using Go.Core.Entities.Order_Aggregate;
using Go.Core.Entities.Product;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


namespace Go.Infrastructure._Data
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Add For All ModelConfigrations.
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Order> Orders { get; set; }    
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }  
        public DbSet<OrderItems> OrderItems { get; set; }   
    }
}
