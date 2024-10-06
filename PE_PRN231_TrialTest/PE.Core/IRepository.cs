using System.Linq.Expressions;

namespace PE.Core
{
    public interface IRepository<T> where T: class, new()
    {
        void Add(T entity);

        void AddRange(IEnumerable<T> entities);

        Task AddAsync(T entity, CancellationToken cancellationToken = default);

        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        void Delete(T entity);

        void DeleteRange(IEnumerable<T> entities);

        void Update(T entity);

        void UpdateRange(IEnumerable<T> entities);

        Task<T?> FindByIdAsync(params object[] keyValues);

        Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        IQueryable<T> GetAll();
    }
}
