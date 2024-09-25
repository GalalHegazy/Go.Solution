using AutoMapper;
using Go.APIs.Dtos.OrderDtos;
using Go.APIs.Errors;
using Go.Core.Entities.Order_Aggregate;
using Go.Core.Services.Contract;
using Microsoft.AspNetCore.Mvc;

namespace Go.APIs.Controllers
{
    //[Authorize]
    public class OrderController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService
                              , IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponce), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderPramDto orderDto)
        {
            var Address = _mapper.Map<ShippingAddressDto, ShippingAddress>(orderDto.shippingAddress);

            var order = await _orderService.CreateOrderAsync(orderDto.BuyerEmail, orderDto.basketId, orderDto.DeliveryMethodId, Address);

            if (order is null) return BadRequest(new APIResponce(400));

            return Ok(_mapper.Map<Order, OrderToReturnDto>(order));
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersByUserEmail(string email)
        {
            //var email = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.GetOrdersForUserAsync(email);

            if (order is null) return NotFound(new APIResponce(404));

            return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(order));
        }
        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponce), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]  // Get api/Order/id?email=
        public async Task<ActionResult<OrderToReturnDto>> GetSpecifiecOrderByUserEmail(int orderId, string email)
        {
            var order = await _orderService.GetOrderByIdForUserAsync(orderId, email);

            if (order is null) return NotFound(new APIResponce(404));

            return Ok(_mapper.Map<Order, OrderToReturnDto>(order));
        }
        [HttpGet("deliverymethods")]
        public async Task<ActionResult<DeliveryMethod>> GetDeliveryMethods()
        {
            var deliverymethods = await _orderService.GetDeliveryMethodsAsync();

            return Ok(deliverymethods);
        }

    }
}
