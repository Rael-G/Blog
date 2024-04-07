using AutoMapper;
using Blog.Application;
using Blog.Domain;
using Moq;

namespace Blog.UnitTest.Application.Services;

public class PostServiceTests
{
    private readonly Mock<IPostRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly PostService _postService;

    private readonly Post _post;

    public PostServiceTests()
    {
        _mockRepository = new Mock<IPostRepository>();
        _mockMapper = new Mock<IMapper>();
        _postService = new PostService(_mockRepository.Object, _mockMapper.Object);

        _post = new Post(Guid.NewGuid(), "Title", "Content");
    }

    [Fact]
    public async void Update_Should_Call_Repository_UpdateAndCommit()
    {
        var dto = new PostDto();

        _mockMapper.Setup(r => r.Map<Post>(It.IsAny<PostDto>()))
            .Returns(_post);

        await _postService.Update(dto);

        _mockRepository.Verify(r => r.Update(_post), Times.Once);
        _mockRepository.Verify(r => r.Commit(), Times.Once);
    }

    [Fact]
    public async void Update_Should_Call_Repository_UpdatePostTag()
    {
        var dto = new PostDto();

        _mockMapper.Setup(r => r.Map<Post>(It.IsAny<PostDto>()))
            .Returns(_post);

        await _postService.Update(dto);

        _mockRepository.Verify(r => r.UpdatePostTag(_post), Times.Once);
    }
}