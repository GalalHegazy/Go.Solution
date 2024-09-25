using Go.Core.Entities;
using System.Linq.Expressions;

namespace Go.Core.Specifications
{
    public interface ISpecifications<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>>? Criteria { set;get;} // SigProp (Where) Like => (P => P.ID == 1)
        public List<Expression<Func<T, object>>> Includes { set;get;} // SigProp (Include) Like => (P => P.brand or category)
        public Expression<Func<T, object>> OrderByAsce { set; get; } // SigProp (OrderByAsce) Like => (P => P.Product)  0 => 10
        public Expression<Func<T, object>> OrderByDesc { set; get; } // SigProp (OrderByDesc) Like => (P => P.Product) 10 => 0
        public int Skip { set; get; } // SigProp (Skip) Like => (10) 
        public int Take { set; get; } // SigProp (Take) Like => (5) 
        public bool IsPaginationEnabled { set; get; } // Flag For Pagination
    }
}
