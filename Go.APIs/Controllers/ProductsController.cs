using AutoMapper;
using Go.APIs.Dtos;
using Go.APIs.Errors;
using Go.APIs.Helpers;
using Go.Core.Entities.Product;
using Go.Core.Services.Contract;
using Go.Core.Specifications.ProductSpec;
using Microsoft.AspNetCore.Mvc;

namespace Go.APIs.Controllers
{
    public class ProductsController : BaseApiController
    {
        //private readonly IGenericRepository<Product> _productRepository;
        //private readonly IGenericRepository<ProductBrand> _brandsRepository;
        //private readonly IGenericRepository<ProductCategory> _categoriesRepository;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(
                                // IGenericRepository<Product> productRepository
                                //,IGenericRepository<ProductBrand> BrandsRepository
                                //,IGenericRepository<ProductCategory> CategoriesRepository
                                IProductService productService
                                , IMapper mapper) 
        {
            //_productRepository = productRepository;
            //_brandsRepository = BrandsRepository;
            //_categoriesRepository = CategoriesRepository;
            _productService = productService;
            _mapper = mapper;
        }
        [CachedAttribute(600)] // 10 min
        [HttpGet]                                                                                 
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts( [FromQuery] AllProductsPram pram)
        {
            var products = await _productService.GetProductsAsync(pram);

            var count = await _productService.GetCountAsync(pram);  

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(new Pagination<ProductToReturnDto>(pram.PageIndex,pram.PageSize,count,data));
        }

        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponce),StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product is null)
                return NotFound(new APIResponce(404));

            return Ok(_mapper.Map<Product,ProductToReturnDto>(product)); 
        }
        [HttpGet("Brands")] // api/products/Brands
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _productService.GetBrandsAsync();

            return Ok(brands);
        }

        [HttpGet("Categories")] // api/products/Categories
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
        {
            var categories = await _productService.GetCategoriesAsync();

            return Ok(categories);
        }
    }
}
