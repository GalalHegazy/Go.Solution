using Go.Core.Entities.Order_Aggregate;

namespace Go.Core.Services.Contract
{
    public interface IOrderService 
    {
        Task<Order?> CreateOrderAsync(string basketId, string buyerEmail, int deliveryMethodId,  ShippingAddress shippingAddress);
        Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);
        Task<Order> GetOrderByIdForUserAsync(int orderId, string buyerEmail);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
    }
}
