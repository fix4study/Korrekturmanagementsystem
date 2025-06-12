using System.Linq.Expressions;
using Korrekturmanagementsystem.Shared;

namespace Korrekturmanagementsystem.Repositories.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        params Expression<Func<TEntity, object>>[] includes);

        Task<TEntity> GetByIdAsync(object id);
        Task<Result<Guid>> InsertAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(object id);
        Task DeleteAsync(TEntity entity);
    }
}
