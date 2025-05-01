using Korrekturmanagementsystem.Data;
using Korrekturmanagementsystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Korrekturmanagementsystem.Repositories;

public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly ApplicationDbContext context;
    protected readonly DbSet<TEntity> dbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        this.context = context;
        this.dbSet = context.Set<TEntity>();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = dbSet;

        if (filter != null)
            query = query.Where(filter);

        foreach (var include in includes)
            query = query.Include(include);

        return orderBy != null
            ? await orderBy(query).ToListAsync()
            : await query.ToListAsync();
    }


    public virtual async Task<TEntity> GetByIdAsync(object id)
    {
        return await dbSet.FindAsync(id);
    }

    public virtual async Task InsertAsync(TEntity entity)
    {
        await dbSet.AddAsync(entity);
    }

    public virtual void Update(TEntity entity)
    {
        dbSet.Attach(entity);
        context.Entry(entity).State = EntityState.Modified;
    }

    public virtual void Delete(object id)
    {
        var entity = dbSet.Find(id);
        if (entity != null)
        {
            Delete(entity);
        }
    }

    public virtual void Delete(TEntity entity)
    {
        if (context.Entry(entity).State == EntityState.Detached)
        {
            dbSet.Attach(entity);
        }

        dbSet.Remove(entity);
    }
}


