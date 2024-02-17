using Blog.Domain.Entities;

namespace Blog.Domain.Interfaces.Repositories;

public interface IBaseRepository<T> where T : BaseEntity
{
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task<T?> Get(Guid id);
    Task<IEnumerable<T>> GetAll();
    Task Commit();
}
