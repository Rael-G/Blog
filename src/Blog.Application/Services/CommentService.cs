using AutoMapper;
using Blog.Domain;

namespace Blog.Application;

public class CommentService(ICommentRepository commentRepository, IMapper mapper)
    : BaseService<CommentDto, Comment>(commentRepository, mapper), ICommentService
{
    private readonly ICommentRepository _commentRepository = commentRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<CommentDto>> GetAll(Guid postId)
    {
        var comment = await _commentRepository.GetAllByPostId(postId);
        return _mapper.Map<IEnumerable<CommentDto>>(comment);
    }
}
