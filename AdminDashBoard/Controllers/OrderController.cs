using AdminDashBoard.ViewModels.Products;
using Go.Core;
using Go.Core.Entities.Order_Aggregate;
using Go.Core.Specifications.OrderSpec;
using Go.Core.Specifications.ProductSpec;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashBoard.Controllers
{
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index(string SearchInputOrder)
        {
            if (string.IsNullOrEmpty(SearchInputOrder))
            {
                var spec = new OrderWithMvcSpec();
                var Orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
                //var MappedProducts = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductViewModel>>(Products);
                return View(Orders);
            }
            else
            {
                var spec = new OrderWithMvcSpec(SearchInputOrder);
                var Order =(Order) _unitOfWork.Repository<Order>().SearchByName(spec);
                //var MappedProduct = _mapper.Map<Product, ProductViewModel>(Product);
                return View(new List<Order>() {Order});
            }
        }
        public async Task<IActionResult> Details(int id)
        {
            var spec = new OrderWithMvcSpec(id);

            var Order = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);

            if (Order is null)
                return BadRequest();

            return View(Order);
        }
    }
}
