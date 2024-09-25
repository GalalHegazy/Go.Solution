using Go.Core.Entities.Basket;
using Go.Core.Entities.Order_Aggregate;

namespace Go.Core.Services.Contract
{
    public interface IPaymentService
    {
        Task<CustomerBasket?> CreateOrUpdatePaymentIntentAsync(string BasketId);

        Task<Order?> UpdateOrderStatus(string paymentIntentId , bool isPaid);
    }
}
