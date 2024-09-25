using Go.Core.Entities.Product;

namespace Go.Core.Specifications.BrandSpec
{
    public class SearchByBrandSpec : BaseSpecifications<ProductBrand>
    {
        public SearchByBrandSpec(string name) : base(e => e.Name.ToLower().Contains(name)){}
        public SearchByBrandSpec(int? id) : base(e => e.Id == id) { }

    }
}
