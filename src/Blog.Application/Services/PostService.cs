using AutoMapper;
using Blog.Domain;

namespace Blog.Application;

public class PostService(IPostRepository postRepository, IMapper mapper)
    : BaseService<PostDto, Post>(postRepository, mapper), IPostService
{
    public const int PageSize = 10;
    private readonly IPostRepository _postRepository = postRepository;

    public override async Task Update(PostDto postDto)
    {
        var post = Mapper.Map<Post>(postDto);
        await _postRepository.UpdatePostTag(post);
        Repository.Update(post);
        await Repository.Commit();
    }

    public async Task<IEnumerable<TagDto>?> GetTags(Guid postId)
    {
        var post = await Repository.Get(postId);

        if (post is null)
            return null;  

        return Mapper.Map<IEnumerable<TagDto>>(post.Tags.Select(pt => pt.Tag));
    }

    public async Task<IEnumerable<PostDto>> GetPage(int page)
    {
        var posts = await _postRepository.GetPage(page, PageSize);
        return Mapper.Map<IEnumerable<PostDto>>(posts);
    }

    public async Task<int> GetPageCount()
        => (int)Math.Ceiling(await _postRepository.GetCount() / (float)PageSize);
}
