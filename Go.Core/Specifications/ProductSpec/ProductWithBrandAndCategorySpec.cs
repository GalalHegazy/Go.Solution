using Go.Core.Entities.Product;

namespace Go.Core.Specifications.ProductSpec
{
    public class ProductWithBrandAndCategorySpec : BaseSpecifications<Product>
    {
        public ProductWithBrandAndCategorySpec(AllProductsPram pram) :
            base(P =>

                  (string.IsNullOrEmpty(pram.Search) || P.Name.ToLower().Contains(pram.Search))  //for Search
                                             &&
                  (!pram.BrandId.HasValue    || P.BrandId == pram.BrandId.Value )
                                             &&
                  (!pram.CategoryId.HasValue || P.CategoryId == pram.CategoryId.Value )
             )
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);


            if (!string.IsNullOrEmpty(pram.Sort))  // Sort By (Name Or Price)  Defult(Name)
            {
                switch (pram.Sort)
                {
                    case "priceAsce":
                        AddOrderByAsce(P => P.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(P => P.Price);
                        break;
                    default:
                        AddOrderByAsce(P => P.Name);
                        break;
                }
            }
            else
                AddOrderByAsce(P => P.Name);

            //prod total =18 => 5 5 5 3  if(size5)
            //pagesize = 5 
            //pageindex =3 => skip(5 5) take(5)  
                                // (3-1)*5 = skip(10)        ,   5  
            ApplyPagination((pram.PageIndex - 1) * pram.PageSize, pram.PageSize);// Enabled Pagination


        }
        public ProductWithBrandAndCategorySpec(int id) : base(P => P.Id == id)
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);
        }
     
     
    }
}
