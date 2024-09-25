namespace Go.Core.Entities.Order_Aggregate
{
    public class Order : BaseEntity // int  Id // (Table)
    {
        public Order() { } // For EFCore
        public Order(string buyerEmail, ShippingAddress address, decimal subTotal, DeliveryMethod? deliveryMethod, ICollection<OrderItems> items,string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            Address = address;
            SubTotal = subTotal;
            DeliveryMethod = deliveryMethod;
            Items = items;
            PaymentIntentId = paymentIntentId;
        }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public ShippingAddress Address { get; set; }
        public decimal SubTotal { get; set; }  // Price * Quantity  , Without DeliveryMethod.Cost
        public decimal GetTotal() => SubTotal + DeliveryMethod.Cost;  // get; (ReadOnly)  That is (Derived attribute)
        public string PaymentIntentId { get; set; } 

        /*public int DeliveryMethodId { get; set; }*/ // FK  Optional  (Allow null)
        public DeliveryMethod? DeliveryMethod { get; set; } // Navegtion Property (One) Relation (1)
        public ICollection<OrderItems> Items { get; set; } = new HashSet<OrderItems>();  // Navegtion Property (Many) Relation (2)
    }
}
