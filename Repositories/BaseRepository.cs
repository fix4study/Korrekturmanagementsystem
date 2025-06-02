using Korrekturmanagementsystem.Data;
using Korrekturmanagementsystem.Repositories.Interfaces;
using Korrekturmanagementsystem.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Korrekturmanagementsystem.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly ApplicationDbContext context;
    protected readonly DbSet<TEntity> dbSet;

    public BaseRepository(ApplicationDbContext context)
    {
        this.context = context;
        this.dbSet = context.Set<TEntity>();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return orderBy != null
            ? await orderBy(query).ToListAsync()
            : await query.ToListAsync();
    }


    public virtual async Task<TEntity> GetByIdAsync(object id) =>
        await dbSet.FindAsync(id);

    public async Task<Result<Guid>> InsertAsync(TEntity entity)
    {
        try
        {
            await context.Set<TEntity>().AddAsync(entity);
            await context.SaveChangesAsync();

            var idProperty = entity.GetType().GetProperty("Id");
            if (idProperty != null)
            {
                var id = (Guid)idProperty.GetValue(entity)!;
                return Result<Guid>.Success(id, "Erfolgreich gespeichert.");
            }
            return Result<Guid>.Failure("Id konnte nicht ermittelt werden.");
        }
        catch (Exception ex)
        {
            return Result<Guid>.Failure("Fehler beim Speichern: " + ex.Message);
        }
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        dbSet.Attach(entity);
        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(object id)
    {
        var entity = await dbSet.FindAsync(id);

        if (entity != null)
        {
            await DeleteAsync(entity);
        }
    }
    public virtual async Task DeleteAsync(TEntity entity)
    {
        if (context.Entry(entity).State == EntityState.Detached)
        {
            dbSet.Attach(entity);
        }

        dbSet.Remove(entity);
        await context.SaveChangesAsync();
    }
}


