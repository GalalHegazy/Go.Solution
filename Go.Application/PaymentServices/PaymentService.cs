using Go.Core;
using Go.Core.Entities.Basket;
using Go.Core.Entities.Order_Aggregate;
using Go.Core.Repositories.Contract;
using Go.Core.Services.Contract;
using Go.Core.Specifications.OrderSpec;
using Microsoft.Extensions.Configuration;
using Stripe;
using Product = Go.Core.Entities.Product.Product;

namespace Go.Application.PaymentServices
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration
                             , IBasketRepository basketRepository
                             , IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntentAsync(string BasketId)
        {
            // Get Secrt Key From AppSetting To Conect With Strip
            StripeConfiguration.ApiKey = _configuration["StripeSettings:Secretkey"];

            // Get Basket By Id 
            var basket = await _basketRepository.GetBasketAsync(BasketId);

            if (basket is null) return null;

            // Check on (price) value in BasketItem From Product 
            if (basket.Items?.Count > 0)
            {
                var productRepo = _unitOfWork.Repository<Product>();

                foreach (var item in basket.Items)
                {
                    var product = await productRepo.GetByIdAsync(item.Id);

                    if (item.Price != product.Price)
                    {
                        item.Price = product.Price;
                    }
                }
            }

            // Get DeliveryMethodCost To sum It With Price To Get TotalPrice
            var shippingPrice = 0m; //Desimal Value

            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);

                shippingPrice = deliveryMethod.Cost;

                basket.ShippingPrice = shippingPrice; // set valua In Prop  ShippingPrice
            }

            // Start Create Or Update To For PaymentIntent
            PaymentIntent paymentIntent;
            PaymentIntentService paymentIntentService = new PaymentIntentService(); // To Craete Options For PaymentIntentId and ClintSecret

            if (string.IsNullOrEmpty(basket.PaymentIntentId)) // Create PaymentIntent
            {
                //To Create PaymentIntentId
                var options = new PaymentIntentCreateOptions()
                {
                    //TotalPrice = (price*quntity)  + shipingprice
                    Amount = (long) basket.Items.Sum(P => P.Price * 100 * P.Quantity) + (long)shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }
                };

                paymentIntent = await paymentIntentService.CreateAsync(options); // PaymentIntent And Go In Strip Transaction to Set Total Amount

                //To set Integration PaymentIntentId and ClientSecret
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else // Update PaymentIntent
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long) basket.Items.Sum(P => P.Price * 100 * P.Quantity) + (long)shippingPrice * 100,
                };

                paymentIntent = await paymentIntentService.UpdateAsync(basket.PaymentIntentId, options); //To set Update Value With Same PaymentIntentId and ClientSecret

            }

            // To Update Basket if There any Updated  
            await _basketRepository.UpdateBasketAsync(basket);

            return basket;
        }

        public async Task<Order?> UpdateOrderStatus(string paymentIntentId, bool isPaid)
        {
            var orderRepo =  _unitOfWork.Repository<Order>();

            var spec = new OrderWithPaymentIntentSpec(paymentIntentId);

            var order = await orderRepo.GetByIdWithSpecAsync(spec);

            if (order is null) return null;

            if (isPaid)
                order.Status = OrderStatus.PaymentReceived;
            else
                order.Status = OrderStatus.PaymentFailed;

            orderRepo.Update(order);

            await _unitOfWork.CompleteAsync();

            return order;

        }
    }
}
