using AutoMapper;
using Blog.Domain;

namespace Blog.Application;

public class TagService(ITagRepository _tagRepository, IMapper mapper)
    : BaseService<TagDto, Tag>(_tagRepository, mapper), ITagService
{
    public const int PageSize = 10;

    public override async Task Create(TagDto tagDto)
    {
        if (await _tagRepository.GetByName(tagDto.Name) is not null) 
            throw new DomainException("Tag Name must be unique");

        await base.Create(tagDto);
    }

    public override async Task Update(TagDto tagDto)
    {
        var existentTag = await _tagRepository.GetByName(tagDto.Name);
        if (existentTag is not null && existentTag.Id != existentTag.Id) 
            throw new DomainException("Tag Name must be unique");

        await base.Update(tagDto);
    }

    public async Task<TagDto?> GetTagPage(Guid id, int page)
    {
        var tag = await Repository.Get(id);

        if (tag == null)
            return null;

        var posts = await _tagRepository.GetTagPage(id, page, PageSize);
        var tagDto = Mapper.Map<TagDto>(tag);
        var postsDto = Mapper.Map<IEnumerable<PostDto>>(posts);
        tagDto.Posts = postsDto;
        return tagDto;
    }

    public async Task<int> GetPageCount(Guid id)
        => (int)Math.Ceiling(await _tagRepository.GetPostCount(id) / (float)PageSize);
}
