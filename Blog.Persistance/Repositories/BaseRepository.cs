using Blog.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Persistance;

public abstract class BaseRepository<T>(ApplicationDbContext context)
    : IBaseRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext Context = context;

    public virtual void Create(T entity)
        => Context.Add(entity);

    public virtual void Update(T entity)
        => Context.Update(entity);

    public virtual void Delete(T entity)
        => Context.Remove(entity);

    public virtual async Task<T?> Get(Guid id)
        => await Context.Set<T>()
        .AsNoTracking()
        .FirstOrDefaultAsync(t => t.Id == id);

    public virtual async Task<IEnumerable<T>> GetAll()
        => await Context.Set<T>()
        .AsNoTracking()
        .ToListAsync();

    public async Task Commit()
        => await Context.SaveChangesAsync();
}
