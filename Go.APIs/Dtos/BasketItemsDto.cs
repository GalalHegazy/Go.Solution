using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace Go.APIs.Dtos
{
    public class BasketItemsDto 
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "Price Must Be Greater Than Zero")]
        public decimal Price { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Qauntity Must Be 1 Item At Least")]
        public int Quantity { get; set; }
        //Qauntity
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Category { get; set; }
    }
}
