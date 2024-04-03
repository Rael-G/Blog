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

    public void Create(TDto dto)
    {
        var entity = _mapper.Map<TEntity>(dto);
        _repository.Create(entity);
    }

    public void Update(TDto dto)
    {
        var entity = _mapper.Map<TEntity>(dto);
        _repository.Update(entity);
    }

    public void Delete(TDto dto)
    {
        var entity = _mapper.Map<TEntity>(dto);
        _repository.Delete(entity);
    }

    public async Task<TDto?> Get(Guid id)
    {
        var entity = await _repository.Get(id);
        return _mapper.Map<TDto>(entity);
    }

    public async Task<IEnumerable<TDto>> GetAll()
    {
        var entity = await _repository.GetAll();
        return _mapper.Map<IEnumerable<TDto>>(entity);
    }

    public async Task Commit()
    {
        await _repository.Commit();
    }
}