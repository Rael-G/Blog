using System.ComponentModel;
using System.Runtime.InteropServices;
using AutoMapper;
using Blog.Domain;

namespace Blog.Application;

public class DomainToDto : Profile
{
    public DomainToDto()
    {
        CreateMap<Comment, CommentDto>()
            .ReverseMap()
            .ForMember(dest => dest.ModifiedTime, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedTime, opt => opt.Ignore());


        CreateMap<Post, PostDto>()
            .ReverseMap()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(tag => new PostTag(src.Id, tag.Id))))
            .ForMember(dest => dest.ModifiedTime, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedTime, opt => opt.Ignore());


        CreateMap<Tag, TagDto>()
            .ReverseMap()
            .ForMember(dest => dest.Posts, opt => opt.MapFrom(src => src.Posts.Select(post => new PostTag(post.Id, src.Id))))
            .ForMember(dest => dest.ModifiedTime, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedTime, opt => opt.Ignore());


        CreateMap<PostTag, TagDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => (src.Tag != null) ? src.Tag.Id : Guid.Empty))
            .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => (src.Tag != null) ? src.Tag.CreatedTime : DateTime.MinValue))
            .ForMember(dest => dest.ModifiedTime, opt => opt.MapFrom(src => (src.Tag != null) ? src.Tag.ModifiedTime: DateTime.MinValue))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => (src.Tag != null) ? src.Tag.Name : string.Empty))
            .ForMember(dest => dest.Posts, opt => opt.Ignore());

        CreateMap<PostTag, PostDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => (src.Post != null) ? src.Post.Id : Guid.Empty))
            .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => (src.Post != null) ? src.Post.CreatedTime : DateTime.MinValue))
            .ForMember(dest => dest.ModifiedTime, opt => opt.MapFrom(src => (src.Post != null) ? src.Post.ModifiedTime : DateTime.MinValue))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => (src.Post != null) ? src.Post.Title : string.Empty))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => (src.Post != null) ? src.Post.Content : string.Empty))
            .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => (src.Post != null) ? src.Post.Comments : new List<Comment>()))
            .ForMember(dest => dest.Tags, opt => opt.Ignore());
    }
}