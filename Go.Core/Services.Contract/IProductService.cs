using Go.Core.Entities.Product;
using Go.Core.Specifications;
using Go.Core.Specifications.ProductSpec;

namespace Go.Core.Services.Contract
{
    public interface IProductService
    {
        Task<IReadOnlyList<Product>> GetProductsAsync(AllProductsPram pram);
        Task<int> GetCountAsync(AllProductsPram pram);
        Task<Product?> GetProductByIdAsync(int id);
        Task<IReadOnlyList<ProductBrand>> GetBrandsAsync();
        Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync();
       
    }
}
