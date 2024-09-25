using Go.Core;
using Go.Core.Entities;
using Go.Core.Repositories.Contract;
using Go.Infrastructure._Data;
using System.Collections;

namespace Go.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _dbContext;
        private Hashtable _repositories;

        public UnitOfWork(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Hashtable();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var Key = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(Key))
            {
                var repository = new GenericRepository<TEntity>(_dbContext); // Create Obj From IGenericRepository

                _repositories.Add(Key, repository);
            }

            return _repositories[Key] as IGenericRepository<TEntity>;
        }
        public async Task<int> CompleteAsync()
                 => await _dbContext.SaveChangesAsync();
        public async ValueTask DisposeAsync()
                 => await _dbContext.DisposeAsync();
    }
}
