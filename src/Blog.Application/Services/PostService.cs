﻿using AutoMapper;
using Blog.Domain;

namespace Blog.Application;

public class PostService(IPostRepository postRepository, IMapper mapper)
    : BaseService<PostDto, Post>(postRepository, mapper), IPostService
{
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

        return Mapper.Map<IEnumerable<TagDto>>(post is not null? post.Tags.Select(pt => pt.Tag) : null);
    }
}
