using Go.Core.Entities.Product;

namespace Go.Core.Specifications.ProductSpec
{
    public class ProductWithFiltertionCountSpec : BaseSpecifications<Product>
    {
        public ProductWithFiltertionCountSpec(AllProductsPram pram) 
            :base(P =>
                  (string.IsNullOrEmpty(pram.Search) || P.Name.ToLower().Contains(pram.Search))
                                          &&
                  (!pram.BrandId.HasValue || P.BrandId == pram.BrandId.Value)
                                             &&
                  (!pram.CategoryId.HasValue || P.CategoryId == pram.CategoryId.Value)
                 )
        {
            //Get All Product (or) All Product With Filteration//
        }
    }
}
