﻿using AutoMapper;
using Blog.Domain;

namespace Blog.Application;

public class CommentService(ICommentRepository commentRepository, IMapper mapper) : ICommentService
{
    private readonly ICommentRepository _commentRepository = commentRepository;
    private readonly IMapper _mapper = mapper;

    public void Create(CommentDto commentDto)
    {
        var comment = _mapper.Map<Comment>(commentDto);
        comment.Validate();

        _commentRepository.Create(comment);
    }

    public void Update(CommentDto commentDto)
    {
        var comment = _mapper.Map<Comment>(commentDto);
        comment.Validate();

        _commentRepository.Update(comment);
    }

    public void Delete(CommentDto commentDto)
    {
        var comment = _mapper.Map<Comment>(commentDto);

        _commentRepository.Delete(comment);
    }

    public async Task<CommentDto?> Get(Guid id)
    {
        var comment = await _commentRepository.Get(id);
        return _mapper.Map<CommentDto>(comment);
    }

    public async Task<IEnumerable<CommentDto>> GetAll()
    {
        var comment = await _commentRepository.GetAll();
        return _mapper.Map<IEnumerable<CommentDto>>(comment);
    }
}
