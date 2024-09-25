using Go.Core;
using Go.Core.Entities.Product;
using Go.Core.Specifications.CategorySpec;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashBoard.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;

        public CategoryController(IUnitOfWork unitOfWork
                                 ,IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _unitOfWork.Repository<ProductCategory>().GetAllAsync();
            return View(categories);
        }
        public async Task<IActionResult> Search(string SearchInputCategory)
        {
            IEnumerable<ProductCategory> categories;

            if (string.IsNullOrEmpty(SearchInputCategory))
            {
                 categories = await _unitOfWork.Repository<ProductCategory>().GetAllAsync();
            }
            else
            {
                var spec = new SearchByCategorySpec(SearchInputCategory);
                categories = _unitOfWork.Repository<ProductCategory>().SearchByName(spec);
            }
            return PartialView("PartialViews/CategoriesTablePartial", categories);

        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCategory productCategory)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            _unitOfWork.Repository<ProductCategory>().Add(productCategory);
            var Count = await _unitOfWork.CompleteAsync();

            if (Count > 0)
                return RedirectToAction(nameof(Index));


            return View(productCategory);
        }
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return NotFound();

            var spec = new SearchByCategorySpec(id);

            var Category = await _unitOfWork.Repository<ProductCategory>().GetByIdWithSpecAsync(spec);

            if (Category is null)
                return BadRequest();

            return View(viewName, Category);
        }
        public async Task<IActionResult> Update(int? id)
        {
            return await Details(id, nameof(Update));
        }
        [HttpPost]
        public async Task<IActionResult> Update(ProductCategory productCategory)
        {

            if (!ModelState.IsValid)
                return View(productCategory);
            try
            {

                _unitOfWork.Repository<ProductCategory>().Update(productCategory);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }

                return View(productCategory);
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, " An Error During Your Update");
            }

            return View(productCategory);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, nameof(Delete));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(ProductCategory productCategory)
        {

            try
            {
                _unitOfWork.Repository<ProductCategory>().Delete(productCategory);
                var Count = await _unitOfWork.CompleteAsync();

                if (Count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }

                return View(productCategory);
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error During Your Delete");
            }

            return View(productCategory);
        }
    }
}
