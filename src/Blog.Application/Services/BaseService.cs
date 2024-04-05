using AutoMapper;
using Blog.Domain;

namespace Blog.Application;

public abstract class BaseService<TDto, TEntity>
    (IBaseRepository<TEntity> repository, IMapper mapper)
    : IBaseService<TDto>
    where TEntity : BaseEntity
{
    private readonly IBaseRepository<TEntity> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public virtual void Create(TDto dto)
    {
        var entity = _mapper.Map<TEntity>(dto);
        _repository.Create(entity);
    }

    public virtual void Update(TDto dto)
    {
        var entity = _mapper.Map<TEntity>(dto);
        _repository.Update(entity);
    }

    public virtual void Delete(TDto dto)
    {
        var entity = _mapper.Map<TEntity>(dto);
        _repository.Delete(entity);
    }

    public virtual async Task<TDto?> Get(Guid id)
    {
        var entity = await _repository.Get(id);
        return _mapper.Map<TDto>(entity);
    }

    public virtual async Task<IEnumerable<TDto>> GetAll()
    {
        var entity = await _repository.GetAll();
        return _mapper.Map<IEnumerable<TDto>>(entity);
    }

    public virtual async Task Commit()
    {
        await _repository.Commit();
    }
}