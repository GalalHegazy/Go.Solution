using Go.Core.Entities;
using Go.Core.Repositories.Contract;

namespace Go.Core
{
    public interface IUnitOfWork : IAsyncDisposable  
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        Task<int> CompleteAsync();
    }
}
