using Go.Core;
using Go.Core.Entities.Product;
using Go.Core.Specifications.BrandSpec;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashBoard.Controllers
{
    public class BrandController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;

        public BrandController(IUnitOfWork unitOfWork
                              ,IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            var brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return View(brands);
        }
        public async Task<IActionResult> Search(string SearchInputBrand)
        {
            IEnumerable<ProductBrand> brands;
            if (string.IsNullOrEmpty(SearchInputBrand))
                 brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
            else
            {
                var spec = new SearchByBrandSpec(SearchInputBrand);
                 brands = _unitOfWork.Repository<ProductBrand>().SearchByName(spec);
            }
            return PartialView("PartialViews/BrandsTablePartial", brands);

        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductBrand productBrand)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            _unitOfWork.Repository<ProductBrand>().Add(productBrand);
            var Count = await _unitOfWork.CompleteAsync();

            if (Count > 0)
                return RedirectToAction(nameof(Index));


            return View(productBrand);
        }
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return NotFound();

            var spec = new SearchByBrandSpec(id);

            var brand = await _unitOfWork.Repository<ProductBrand>().GetByIdWithSpecAsync(spec);

            if (brand is null)
                return BadRequest();

            return View(viewName, brand);
        }
        public async Task<IActionResult> Update(int? id)
        {
            return await Details(id, nameof(Update));
        }
        [HttpPost]
        public async Task<IActionResult> Update(ProductBrand productBrand)
        {

            if (!ModelState.IsValid)
                return View(productBrand);
            try
            {

                _unitOfWork.Repository<ProductBrand>().Update(productBrand);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }

                return View(productBrand);
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, " An Error During Your Update");
            }

            return View(productBrand);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, nameof(Delete));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(ProductBrand productBrand)
        {

            try
            {
                _unitOfWork.Repository<ProductBrand>().Delete(productBrand);
                var Count = await _unitOfWork.CompleteAsync();

                if (Count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }

                return View(productBrand);
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error During Your Delete");
            }

            return View(productBrand);
        }

    }
}
