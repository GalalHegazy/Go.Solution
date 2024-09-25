namespace Go.Core.Entities.Order_Aggregate
{
    public class OrderItems : BaseEntity  // int  Id  // (Table)
    {
        private OrderItems(){} // For EFCore
        public OrderItems(ProductItemOrdered product, decimal price, int quantity)
        {
            Product = product;
            Price = price;
            Quantity = quantity;
        }

        public ProductItemOrdered Product { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        /*public int OrderId  { get; set; } */ // FK   
        /*public Order Order { get; set; }*/  // Navegtion Property (One) Relation (2)  
    }
}
