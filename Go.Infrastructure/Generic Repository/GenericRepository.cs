using Go.Core.Entities;
using Go.Core.Repositories.Contract;
using Go.Core.Specifications;
using Go.Infrastructure._Data;
using Microsoft.EntityFrameworkCore;

namespace Go.Infrastructure
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreDbContext _dbContext;

        public GenericRepository(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
        {
            return await SpecificationsEvaluator<T>.GetQuery(_dbContext.Set<T>(),spec).AsNoTracking().ToListAsync(); 
        }
        public async Task<T?> GetByIdWithSpecAsync(ISpecifications<T> spec)
        {
            return await SpecificationsEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec).FirstOrDefaultAsync();
        }
        public async Task<int> GetCountAsync(ISpecifications<T> spec)  // null (Count Of All Product) (or) (Count Of Product with Filtartion) p=>p.id==id 
        {
            return await SpecificationsEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec).CountAsync();
        }
        public IQueryable<T> SearchByName(ISpecifications<T> spec)
        {
            return SpecificationsEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec).AsNoTracking();
        }

        public void Add(T TEntity)
                  => _dbContext.Set<T>().Add(TEntity);  

        public void Update(T TEntity)
                  => _dbContext.Set<T>().Update(TEntity);
        public void Delete(T TEntity)
                  => _dbContext.Set<T>().Remove(TEntity);

   
    }
}
