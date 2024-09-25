using Go.Core.Entities.Order_Aggregate;

namespace Go.APIs.Dtos.OrderDtos
{
    public class OrderToReturnDto
    {
        public int Id { get; set; } 
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } 
        public String Status { get; set; } // To set String Label Not Eunm Value (like 0, 1) 
        public ShippingAddress Address { get; set; } 
        public decimal SubTotal { get; set; }  // Price * Quantity  , Without DeliveryMethod.Cost
        public decimal Total { get; set; }   // Get Total Map By Convetion  From GetTotal() (Derived attribute)
        public string PaymentIntentId { get; set; } = string.Empty; //مؤقتا   
        public string DeliveryMethod { get; set; } // To Map by FluntApi from Nav Prop DeliveryMethod
        public decimal DeliveryMethodCost { get; set; } // To Map by FluntApi from Nav Prop DeliveryMethod
        public ICollection<OrderItemsDto> Items { get; set; } = new HashSet<OrderItemsDto>();  //  To Map by FluntApi from Nav Prop OrderItems
    }
}
