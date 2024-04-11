using AutoMapper;
using Blog.Domain;

namespace Blog.Application;

public class TagService(ITagRepository tagRepository, IMapper mapper)
    : BaseService<TagDto, Tag>(tagRepository, mapper), ITagService
{
    public const int PageSize = 10;
    public async Task<TagDto?> GetTagPage(Guid id, int page)
    {
        var tag = await Repository.Get(id);

        if (tag == null)
            return null;

        var posts = await tagRepository.GetTagPage(id, page, PageSize);
        var tagDto = Mapper.Map<TagDto>(tag);
        var postsDto = Mapper.Map<IEnumerable<PostDto>>(posts);
        tagDto.Posts = postsDto;
        return tagDto;
    }

    public async Task<int> GetPageCount(Guid id)
        => (int)Math.Ceiling(await tagRepository.GetPostCount(id) / (float)PageSize);
}
