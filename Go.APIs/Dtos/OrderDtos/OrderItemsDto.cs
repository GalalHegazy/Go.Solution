namespace Go.APIs.Dtos.OrderDtos
{
    public class OrderItemsDto
    {      
        public int Id { get; set; }  // Order Item Id 
        public int ProductId { get; set; } // Product Id in order Item In order
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}