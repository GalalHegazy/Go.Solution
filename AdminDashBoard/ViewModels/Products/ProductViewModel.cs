using Go.Core.Entities.Product;
using System.ComponentModel.DataAnnotations;

namespace AdminDashBoard.ViewModels.Products
{
    public class ProductViewModel
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "Name Is Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description Is Required")]
        public string Description { get; set; }
        public IFormFile? Image { get; set; }
        public string? PictureUrl { get; set; }
        [Required(ErrorMessage = "Price Is Required")]
        [Range(1,100000)]
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public ProductCategory? Category { get; set; }
        public int BrandId { get; set; }
        public ProductBrand? Brand { get; set; }


    }
}
