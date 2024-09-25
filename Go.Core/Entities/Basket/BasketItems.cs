namespace Go.Core.Entities.Basket
{
    public class BasketItems
    {
        public int Id { get; set; } 
        public string ProductName { get; set; }    
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }  
        public int Quantity { get; set; }
        //Qauntity
        public string Brand  { get; set; }
        public string Category { get; set; }

    }
}