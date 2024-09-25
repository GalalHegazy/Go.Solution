using Go.Core.Entities;
using Go.Core.Specifications;

namespace Go.Core.Repositories.Contract
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec);
        Task<T?> GetByIdWithSpecAsync(ISpecifications<T> spec);
        Task<int> GetCountAsync(ISpecifications<T> spec);
        IQueryable<T> SearchByName(ISpecifications<T> spec);
        void Add(T TEntity);
        void Update(T TEntity);
        void Delete(T TEntity);
    }
}
