using AutoMapper;
using Go.Core.Entities.Basket;
using System.ComponentModel.DataAnnotations;

namespace Go.APIs.Dtos
{
    public class CustomerBasketDto 
    {
        [Required]
        public String Id { get; set; }
        [Required]
        public List<BasketItemsDto> Items { get; set; }
        public string? PaymentIntentId { get; set; }  // PaymentIntent
        public string? ClientSecret { get; set; }     // PaymentIntent
        public int? DeliveryMethodId { get; set; }    // PaymentIntent
        public decimal? ShippingPrice { get; set; } // PaymentIntent
    }
}
