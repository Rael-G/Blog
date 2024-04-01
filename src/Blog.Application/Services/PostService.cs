using AutoMapper;
using Blog.Domain;

namespace Blog.Application;

public class PostService(IPostRepository postRepository, IMapper mapper) : IPostService
{
    private readonly IPostRepository _postRepository = postRepository;
    private readonly IMapper _mapper = mapper;

    public void Create(PostDto postDto)
    {
        var post = _mapper.Map<Post>(postDto);

        _postRepository.Create(post);
    }

    public void Update(PostDto postDto)
    {
        var post = _mapper.Map<Post>(postDto);
        post.UpdateTime();

        _postRepository.Update(post);
    }

    public void Delete(PostDto postDto)
    {
        var post = _mapper.Map<Post>(postDto);

        _postRepository.Delete(post);
    }

    public async Task<PostDto?> Get(Guid id)
    {
        var post = await _postRepository.Get(id);
        return _mapper.Map<PostDto>(post);
    }

    public async Task<IEnumerable<PostDto>> GetAll()
    {
        var post = await _postRepository.GetAll();
        return _mapper.Map<IEnumerable<PostDto>>(post);
    }

    public async Task Commit()
    {
        await _postRepository.Commit();
    }
}
