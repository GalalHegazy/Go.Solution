
namespace Go.Core.Entities.Basket
{
    public class CustomerBasket
    {
        public String Id { get; set; }
        public List<BasketItems> Items { get; set; }
        public string? PaymentIntentId { get; set; }  // PaymentIntent
        public string? ClientSecret { get; set; }     // PaymentIntent
        public int? DeliveryMethodId { get; set; }    // PaymentIntent
        public decimal? ShippingPrice { get; set; }   // PaymentIntent

        public CustomerBasket(string id)
        {
            Id = id;
            Items = new List<BasketItems>();
        }
    }
}
