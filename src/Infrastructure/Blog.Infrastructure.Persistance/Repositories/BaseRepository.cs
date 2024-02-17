using Blog.Domain.Entities;
using Blog.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Persistance;

public abstract class BaseRepository<T>(ApplicationDbContext context) : IBaseRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext Context = context;

    public void Create(T entity)
        => Context.Add(entity);
    
    public void Update(T entity)
        => Context.Update(entity);
    
    public void Delete(T entity)
        => Context.Remove(entity);
    
    public async Task<T?> Get(Guid id)
        => await Context.Set<T>().FirstOrDefaultAsync(t => t.Id == id);

    public async Task<IEnumerable<T>> GetAll()
        => await Context.Set<T>().ToListAsync();

    public async Task Commit()
        => await Context.SaveChangesAsync();
}
