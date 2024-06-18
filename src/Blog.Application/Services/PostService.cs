using AutoMapper;
using Blog.Domain;

namespace Blog.Application;

public class PostService(IPostRepository _postRepository, IMapper mapper)
    : BaseService<PostDto, Post>(_postRepository, mapper), IPostService
{
    public const int PageSize = 10;

    public override async Task Create(PostDto postDto)
    {
        if (await _postRepository.GetByTitle(postDto.Title) is not null) 
            throw new DomainException("Post Title must be unique");

        await base.Create(postDto);
    }

    public override async Task Update(PostDto postDto)
    {
        var existentPost = await _postRepository.GetByTitle(postDto.Title);
        if (existentPost is not null && existentPost.Id != postDto.Id) 
            throw new DomainException("Post Title must be unique");

        var post = Mapper.Map<Post>(postDto);
        await _postRepository.UpdatePostTag(post);
        Repository.Update(post);
        await Repository.Commit();
    }

    public async Task<IEnumerable<PostDto>> GetPage(int page)
    {
        var posts = await _postRepository.GetPage(page, PageSize);
        return Mapper.Map<IEnumerable<PostDto>>(posts);
    }

    public async Task<int> GetPageCount()
        => (int)Math.Ceiling(await _postRepository.GetCount() / (float)PageSize);
}
