using AutoMapper;
using Blog.Domain;

namespace Blog.Application;

public class PostService(IPostRepository postRepository, IMapper mapper)
    : BaseService<PostDto, Post>(postRepository, mapper), IPostService
{
}
