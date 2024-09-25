
namespace Go.Core.Specifications.ProductSpec
{
    public class AllProductsPram
    {
        public string? Sort { get; set; }
        public int? BrandId { get; set; }   
        public int? CategoryId { get; set; }
        public int PageIndex { get; set; } = 1;  // If Value Come Null  Defult(page 1 it have a Defult pagesize(5 product))

        private const int maxPageValue = 10;
        private int pageSize = 5; // If Value Come Null  Defult(5 product)
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > maxPageValue ? maxPageValue : value; } // Validation On Value Come From 
        }
        private string? search;
        public string? Search
        {
            get { return search; }
            set { search = value?.ToLower();}
        }


    }
}
