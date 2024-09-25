using Go.Core.Entities.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Go.Core.Specifications.CategorySpec
{
    public class SearchByCategorySpec :BaseSpecifications<ProductCategory>
    {
        public SearchByCategorySpec(string name) : base(e => e.Name.ToLower().Contains(name)) { }

        public SearchByCategorySpec(int? id) : base(e => e.Id == id) { }
    }
}
