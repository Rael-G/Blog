using AutoMapper;
using Blog.Domain;

namespace Blog.Application;

public class PostService(IPostRepository postRepository, IMapper mapper)
    : BaseService<PostDto, Post>(postRepository, mapper), IPostService
{
    public const int PageSize = 10;

    public override async Task Update(PostDto postDto)
    {
        var post = Mapper.Map<Post>(postDto);
        await postRepository.UpdatePostTag(post);
        Repository.Update(post);
        await Repository.Commit();
    }

    public async Task<IEnumerable<PostDto>> GetPage(int page)
    {
        var posts = await postRepository.GetPage(page, PageSize);
        return Mapper.Map<IEnumerable<PostDto>>(posts);
    }

    public async Task<int> GetPageCount()
        => (int)Math.Ceiling(await postRepository.GetCount() / (float)PageSize);
}
