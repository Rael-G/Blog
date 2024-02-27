using AutoMapper;
using Blog.Domain;

namespace Blog.Application;

public class DomainToDto : Profile
{
    public DomainToDto()
    {
        CreateMap<Post, PostDto>().ReverseMap();
        CreateMap<Comment, CommentDto>().ReverseMap();
    }
}
