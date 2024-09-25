
namespace Go.Core.Entities.Order_Aggregate
{
    public class ProductItemOrdered  // (Component (Composite attribute for OrderItems Table))
    {
        private ProductItemOrdered(){} // For EFCore
        public ProductItemOrdered(int productId, string productName, string pictureUrl)
        {
            ProductId = productId;
            ProductName = productName;
            PictureUrl = pictureUrl;
        }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
    }
}
