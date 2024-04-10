using AutoMapper;
using Blog.Domain;

namespace Blog.Application;

public class TagService(ITagRepository tagRepository, IMapper mapper)
    : BaseService<TagDto, Tag>(tagRepository, mapper), ITagService
{
    private readonly ITagRepository _tagRepository = tagRepository;
    public async Task<IEnumerable<TagDto>> GetAll(Guid postId)
    {
        var tags = await _tagRepository.GetAll(postId);
        return Mapper.Map<IEnumerable<TagDto>>(tags);
    }
}
