using Go.Core;
using Go.Core.Entities.Order_Aggregate;
using Go.Core.Entities.Product;
using Go.Core.Repositories.Contract;
using Go.Core.Services.Contract;
using Go.Core.Specifications.OrderSpec;

namespace Go.Application.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        //private readonly IGenericRepository<Product> _productRepo;
        //private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
        //private readonly IGenericRepository<Order> _orderRepo;

        public OrderService(IBasketRepository basketRepo
                          //,IGenericRepository<Product> productRepo
                          //,IGenericRepository<DeliveryMethod> deliveryMethodRepo
                          //,IGenericRepository<Order> orderRepo
                          , IUnitOfWork unitOfWork
                          , IPaymentService paymentService  
                           )
        {
            _basketRepo = basketRepo;
            //_productRepo = productRepo;
            //_deliveryMethodRepo = deliveryMethodRepo;
            //_orderRepo = orderRepo;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, ShippingAddress shippingAddress )
        {
            // 1.Get Basket From Baskets Repo

            var basket = await _basketRepo.GetBasketAsync(basketId); //Return (String Id ,List<BasketItems> Items)

            // 2. Get Selected Items at Basket From Products Repo
            var orderItems = new List<OrderItems>();

            if (basket?.Items?.Count > 0)
            {
                var productRepo = _unitOfWork.Repository<Product>();

                foreach (var item in basket.Items)
                {
                    var product = await productRepo.GetByIdAsync(item.Id); // Get Product By basketId

                    var productItemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl); //  Create Product Item Ordered By Data from Product

                    var orderItem = new OrderItems(productItemOrdered, product.Price, item.Quantity); //  Create Order Item  By Data from Product and basket

                    orderItems.Add(orderItem);
                }
            }
            // 3. Calculate SubTotal

            var subTotal = orderItems.Sum(S => S.Price * S.Quantity);

            // 4. Get Delivery Method From DeliveryMethods Repo

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            // 5. Check If It Have any copied paymentIntentId
            var orderRepo = _unitOfWork.Repository<Order>();

            var spec = new OrderWithPaymentIntentSpec(basket.PaymentIntentId);

            var existingOrder = await orderRepo.GetByIdWithSpecAsync(spec);

            if (existingOrder is not null)
            {
                orderRepo.Delete(existingOrder); // if any daplicated delete
                await _paymentService.CreateOrUpdatePaymentIntentAsync(basketId); // Update the amount if it not match
            }

            // 6. Create Order

            var order = new Order(
                 buyerEmail: buyerEmail,
                 address: shippingAddress,
                 deliveryMethod: deliveryMethod,
                 subTotal: subTotal,
                 items: orderItems,
                 paymentIntentId : basket.PaymentIntentId ?? ""
                 );

            orderRepo.Add(order); // Add Order To DataBase (change mark For entity To Added)

            // 7. Save To Database [TODO]
            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0) return null;

            return order;
        }
        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrderWithSpec(buyerEmail);

            var order = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);

            return order;   
        }
        public async Task<Order?> GetOrderByIdForUserAsync(int orderId, string buyerEmail)
        {
            var spec = new OrderWithSpec(orderId, buyerEmail);

            var order = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);

            return order;
        }
        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
                    => await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
    }
}
