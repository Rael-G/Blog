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
    public async void Update_Should_Call_Repository_Update()
    {
        var dto = new PostDto();

        _mockMapper.Setup(r => r.Map<Post>(It.IsAny<PostDto>()))
            .Returns(_post);

        await _postService.Update(dto);

        _mockRepository.Verify(r => r.Update(_post), Times.Once);
    }

    [Fact]
    public async void Update_Should_Call_Repository_Commit()
    {
        var dto = new PostDto();

        _mockMapper.Setup(r => r.Map<Post>(It.IsAny<PostDto>()))
            .Returns(_post);

        await _postService.Update(dto);

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

    [Fact]
    public async void GetPage_Should_Call_Repository_GetPage_WithPageAndPageSize()
    {
        var page = 10;

        _mockMapper.Setup(r => r.Map<IEnumerable<PostDto>>(It.IsAny<IEnumerable<Post>>()))
            .Returns([]);

        await _postService.GetPage(page);

        _mockRepository.Verify(r => r.GetPage(page, PostService.PageSize), Times.Once);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(687906)]
    public async void GetPageCount_Should_Returns_CorrectPageCount_ForGivenTotalPosts(int count)
    {
        _mockRepository.Setup(r => r.GetCount())
            .ReturnsAsync(count);

        var result = await _postService.GetPageCount();

        var expectedResult = (int)Math.Ceiling(count / (float)TagService.PageSize);
        Assert.Equal(result, expectedResult);
    }

    [Fact]
    public async void GetPageCount_Should_Call_Repository_GetCount()
    {
        await _postService.GetPageCount();
        _mockRepository.Verify(r => r.GetCount(), Times.Once);
    }
}