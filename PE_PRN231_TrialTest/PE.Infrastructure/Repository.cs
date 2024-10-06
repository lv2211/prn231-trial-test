using Microsoft.EntityFrameworkCore;
using PE.Core;
using PE.Infrastructure.Databases;
using System.Linq.Expressions;

namespace PE.Infrastructure
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        protected readonly EnglishPremierLeague2024DbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public Repository(EnglishPremierLeague2024DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public void Add(T entity) => _dbContext.Add(entity);

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
            => await _dbContext.AddAsync(entity, cancellationToken);

        public void AddRange(IEnumerable<T> entities) => _dbContext.AddRange(entities);
        
        public Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
            => _dbContext.AddRangeAsync(entities, cancellationToken);

        public void Delete(T entity) => _dbContext.Remove(entity);

        public void DeleteRange(IEnumerable<T> entities) => _dbContext.RemoveRange(entities);

        public async Task<T?> FindByIdAsync(params object[] keyValues)
            => await _dbSet.FindAsync(keyValues);

        public IQueryable<T> GetAll() => _dbSet.AsQueryable();

        public Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);

        public void Update(T entity) => _dbContext.Update(entity);

        public void UpdateRange(IEnumerable<T> entities) => _dbContext.UpdateRange(entities);
    }
}
