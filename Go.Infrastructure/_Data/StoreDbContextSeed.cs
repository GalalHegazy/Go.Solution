using Go.Core.Entities.Order_Aggregate;
using Go.Core.Entities.Product;
using System.Text.Json;

namespace Go.Infrastructure._Data
{
    public static class StoreDbContextSeed
    {
        public static async Task SeedAsync(StoreDbContext _dbContext)
        {
            #region Product Data 
            if (_dbContext.ProductBrands.Count() == 0)
            {
                var brandData = File.ReadAllText("../Go.Infrastructure/_Data/DataSeed/brands.json"); // Catch Json File 
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData); // Transorm From Json To List Of Obj

                if (brands?.Count > 0)
                {
                    foreach (var brand in brands)
                    {
                        _dbContext.Set<ProductBrand>().Add(brand);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
            if (_dbContext.ProductCategories.Count() == 0)
            {
                var categoriesData = File.ReadAllText("../Go.Infrastructure/_Data/DataSeed/categories.json"); // Catch Json File 
                var categories = JsonSerializer.Deserialize<List<ProductCategory>>(categoriesData); // Transform From Json To List Of Obj

                if (categories?.Count > 0)
                {
                    foreach (var category in categories)
                    {
                        _dbContext.Set<ProductCategory>().Add(category);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
            if (_dbContext.Products.Count() == 0)
            {
                var productsData = File.ReadAllText("../Go.Infrastructure/_Data/DataSeed/products.json"); // Catch Json File 
                var products = JsonSerializer.Deserialize<List<Product>>(productsData); // Transform From Json To List Of Obj

                if (products?.Count > 0)
                {
                    foreach (var product in products)
                    {
                        _dbContext.Set<Product>().Add(product);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
            #endregion
            #region Order Data 
            if (_dbContext.DeliveryMethods.Count() == 0)
            {
                var DeliveryMethodsData = File.ReadAllText("../Go.Infrastructure/_Data/DataSeed/delivery.json"); // Catch Json File 
                var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodsData); // Transform From Json To List Of Obj

                if (DeliveryMethods?.Count > 0)
                {
                    foreach (var DeliveryMethod in DeliveryMethods)
                    {
                        _dbContext.Set<DeliveryMethod>().Add(DeliveryMethod);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            } 
            #endregion
        }
    }
}
