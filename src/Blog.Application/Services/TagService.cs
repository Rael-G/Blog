using AutoMapper;
using Blog.Domain;

namespace Blog.Application;

public class TagService(ITagRepository tagRepository, IMapper mapper)
    : BaseService<TagDto, Tag>(tagRepository, mapper), ITagService
{
}
