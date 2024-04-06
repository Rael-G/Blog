using AutoMapper;
using Blog.Domain;

namespace Blog.Application;

public abstract class BaseService<TDto, TEntity>
    (IBaseRepository<TEntity> repository, IMapper mapper)
    : IBaseService<TDto>
    where TEntity : BaseEntity
{
    protected readonly IBaseRepository<TEntity> Repository = repository;
    protected readonly IMapper Mapper = mapper;

    public virtual void Create(TDto dto)
    {
        var entity = Mapper.Map<TEntity>(dto);
        Repository.Create(entity);
    }

    public virtual void Update(TDto dto)
    {
        var entity = Mapper.Map<TEntity>(dto);
        Repository.Update(entity);
    }

    public virtual void Delete(TDto dto)
    {
        var entity = Mapper.Map<TEntity>(dto);
        Repository.Delete(entity);
    }

    public virtual async Task<TDto?> Get(Guid id)
    {
        var entity = await Repository.Get(id);
        return Mapper.Map<TDto>(entity);
    }

    public virtual async Task<IEnumerable<TDto>> GetAll()
    {
        var entity = await Repository.GetAll();
        return Mapper.Map<IEnumerable<TDto>>(entity);
    }

    public virtual async Task Commit()
    {
        await Repository.Commit();
    }
}