using AdminDashBoard.Helpers;
using AdminDashBoard.ViewModels.Products;
using AutoMapper;
using Go.Core;
using Go.Core.Entities.Product;
using Go.Core.Specifications.ProductSpec;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashBoard.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public ProductController(IUnitOfWork unitOfWork
                                , IMapper mapper
                                , IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            var spec = new ProductWithBrandAndCategoryMvcSpec();
            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);
            var mappedProducts = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(products);
            return View(mappedProducts);
        }
        public async Task<IActionResult> Search(string SearchInputProd)
        {
            IEnumerable<Product> products;
            var spec = new ProductWithBrandAndCategoryMvcSpec();

            if (string.IsNullOrEmpty(SearchInputProd))
                products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);
            else
            {
                spec = new ProductWithBrandAndCategoryMvcSpec(SearchInputProd);
                products = _unitOfWork.Repository<Product>().SearchByName(spec);
            }
            var mappedProducts = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(products);

            return PartialView("PartialViews/ProductTablePartial", mappedProducts);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel productVM)
        {

            productVM.PictureUrl = await DocumentSettings.UploadFileAsync(productVM.Image, "products");

            if (!ModelState.IsValid)
                return BadRequest();

            var MappedProductVM = _mapper.Map<ProductViewModel, Product>(productVM);
            _unitOfWork.Repository<Product>().Add(MappedProductVM);
            var Count = await _unitOfWork.CompleteAsync();
            if (Count > 0)
                return RedirectToAction(nameof(Index));
            else
                DocumentSettings.DeleteFile(productVM.PictureUrl, "products");

            return View(productVM);
        }
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return NotFound();

            var spec = new ProductWithBrandAndCategoryMvcSpec(id);

            var Product = await _unitOfWork.Repository<Product>().GetByIdWithSpecAsync(spec);


            if (Product is null)
                return BadRequest();

            var mappedProduct = _mapper.Map<Product, ProductViewModel>(Product);

            if (viewName.Equals("Delete", StringComparison.OrdinalIgnoreCase) || viewName.Equals("Update", StringComparison.OrdinalIgnoreCase))
            {
                TempData["ImageName"] = Product.PictureUrl;
                ViewData["Image"] = Product.PictureUrl;
            }

            return View(viewName, mappedProduct);
        }
        public async Task<IActionResult> Update(int? id)
        {
            return await Details(id, nameof(Update));
        }
        [HttpPost]
        public async Task<IActionResult> Update(ProductViewModel productVM)
        {
            var oldImg = TempData["ImageName"] as string;

            if (!ModelState.IsValid)
                return View(productVM);

            if (productVM.Image is not null)
                productVM.PictureUrl = await DocumentSettings.UploadFileAsync(productVM.Image, "products");


            var mappedProduct = _mapper.Map<ProductViewModel, Product>(productVM);

            try
            {

                _unitOfWork.Repository<Product>().Update(mappedProduct);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    DocumentSettings.DeleteFile(oldImg, "products");
                    return RedirectToAction(nameof(Index));
                }

                return View(productVM);
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, " An Error During Your Update");
            }

            return View(productVM);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, nameof(Delete));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(ProductViewModel productVM)
        {
            productVM.PictureUrl = TempData["ImageName"] as string;

            var mappedProduct = _mapper.Map<ProductViewModel, Product>(productVM);
            try
            {
                _unitOfWork.Repository<Product>().Delete(mappedProduct);
                var Count = await _unitOfWork.CompleteAsync();

                if (Count > 0)
                {

                    DocumentSettings.DeleteFile(productVM.PictureUrl, "products");
                    return RedirectToAction(nameof(Index));

                }

                return View(productVM);
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error During Your Delete");
            }

            return View(productVM);
        }


    }
}
