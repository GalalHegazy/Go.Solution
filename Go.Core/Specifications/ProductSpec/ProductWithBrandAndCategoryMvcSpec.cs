using Go.Core.Entities.Product;


namespace Go.Core.Specifications.ProductSpec
{
    public class ProductWithBrandAndCategoryMvcSpec : BaseSpecifications<Product>
    {
        public ProductWithBrandAndCategoryMvcSpec() 
        {
            Includes.Add(D => D.Brand);
            Includes.Add(D => D.Category);
        }
        public ProductWithBrandAndCategoryMvcSpec(string name) : base(e => e.Name.ToLower().Contains(name))
        {
            Includes.Add(D => D.Brand);
            Includes.Add(D => D.Category);
        }
        public ProductWithBrandAndCategoryMvcSpec(int? Id) : base(P => P.Id == Id)
        {
            Includes.Add(D => D.Brand);
            Includes.Add(D => D.Category);
        }
    }
}
