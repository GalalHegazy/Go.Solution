using System.ComponentModel.DataAnnotations;

namespace Go.APIs.Dtos.OrderDtos
{
    public class OrderPramDto
    {
        [Required]
        public string basketId { get; set; }
        [Required]
        public string BuyerEmail { get; set; }
        [Required]
        public int DeliveryMethodId { get; set; }
        public ShippingAddressDto shippingAddress { get; set; }
    }
}
