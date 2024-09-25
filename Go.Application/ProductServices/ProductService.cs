using Go.Core;
using Go.Core.Entities.Product;
using Go.Core.Services.Contract;
using Go.Core.Specifications;
using Go.Core.Specifications.ProductSpec;

namespace Go.Application.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IReadOnlyList<Product>> GetProductsAsync(AllProductsPram pram)
        {
            var spec = new ProductWithBrandAndCategorySpec(pram);

            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);

            return products;    
        }
        public async Task<int> GetCountAsync(AllProductsPram pram)
        {
            var countSpec = new ProductWithFiltertionCountSpec(pram);

            var count = await _unitOfWork.Repository<Product>().GetCountAsync(countSpec);

            return count;
        }
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            var spec = new ProductWithBrandAndCategorySpec(id);

            var product = await _unitOfWork.Repository<Product>().GetByIdWithSpecAsync(spec);

            return product;
        }
        public async Task<IReadOnlyList<ProductBrand>> GetBrandsAsync()
                     => await _unitOfWork.Repository<ProductBrand>().GetAllAsync();

        public async Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync()
                     => await _unitOfWork.Repository<ProductCategory>().GetAllAsync();


    }
}
