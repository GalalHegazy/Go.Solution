using Go.Core.Entities;
using System.Linq.Expressions;


namespace Go.Core.Specifications
{
    public class BaseSpecifications<T> : ISpecifications<T> where T : BaseEntity
    {
        // This Class For Create Obj From Sepc 
        public Expression<Func<T, bool>>? Criteria { get; set; } // For (Where) 
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>(); //For (Include)
        public Expression<Func<T, object>> OrderByAsce { get; set ; } //For (OrderByAsce)
        public Expression<Func<T, object>> OrderByDesc { get; set ; } //For (OrderByDesc)
        public int Skip { get; set ; }// For (Skip)
        public int Take { get ; set; }// For (Take)
        public bool IsPaginationEnabled { get ; set; } // Flage For Apply Pagination

        // For Get All Items<T>
        public BaseSpecifications()
        {
            //Criteria = null;
            //Includes = new List<Expression<Func<T, object>>>();
        }
        // For Get Item<T> By Id
        public BaseSpecifications(Expression<Func<T, bool>> criteriaExpression) 
        {
            Criteria = criteriaExpression;
            //Includes = new List<Expression<Func<T, object>>>();
        }

        // Just Two Mathod For Set Expression To OrderByAsce and OrderByDesc
        public void AddOrderByAsce(Expression<Func<T, object>> OrderByAsceExpression)
        {
            OrderByAsce = OrderByAsceExpression;
        }
        public void AddOrderByDesc(Expression<Func<T, object>> OrderByDescExpression)
        {
            OrderByDesc = OrderByDescExpression;
        }

        public void ApplyPagination(int skip ,int take)
        {
            IsPaginationEnabled = true;
            Skip = skip;
            Take = take;
        }
    }
}
