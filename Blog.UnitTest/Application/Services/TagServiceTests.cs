using AutoMapper;
using Blog.Application;
using Blog.Domain;
using FluentAssertions;
using Moq;

namespace Blog.UnitTest.Application.Services;

public class TagServiceTests
{
    private readonly Mock<ITagRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly TagService _tagService;

    public TagServiceTests()
    {
        _mockRepository = new Mock<ITagRepository>();
        _mockMapper = new Mock<IMapper>();
        _tagService = new TagService(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async void GetTagPage_Should_Call_Repository_GetTagPage_WithPageAndPageSize()
    {
        var page = 10;
        var id = Guid.NewGuid();
        var tagDto = new TagDto();
        var posts = new List<Post>(){};
        var tag = new Tag(Guid.NewGuid(), "TagName");

        _mockRepository.Setup(r => r.Get(It.IsAny<Guid>()))
            .ReturnsAsync(tag);
        _mockMapper.Setup(m => m.Map<TagDto>(It.IsAny<Tag>()))
            .Returns(tagDto);
        _mockRepository.Setup(r => r.GetTagPage(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(posts);
        _mockMapper.Setup(m => m.Map<IEnumerable<PostDto>>(It.IsAny<IEnumerable<Post>>()))
            .Returns([]);

        await _tagService.GetTagPage(id, page);

        _mockRepository.Verify(r => r.GetTagPage(id, page, PostService.PageSize), Times.Once);
    }

    [Fact]
    public async void GetTagPage_Should_Return_TagWithPosts()
    {
        var page = 10;
        var id = Guid.NewGuid();
        var postId = Guid.NewGuid();
        var tagDto = new TagDto();
        var posts = new List<Post>(){};
        var postDtos = new List<PostDto>(){new PostDto{Id = Guid.NewGuid()}};
        var tag = new Tag(Guid.NewGuid(), "TagName");

        _mockRepository.Setup(r => r.Get(It.IsAny<Guid>()))
            .ReturnsAsync(tag);
        _mockMapper.Setup(m => m.Map<TagDto>(It.IsAny<Tag>()))
            .Returns(tagDto);
        _mockRepository.Setup(r => r.GetTagPage(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(posts);
        _mockMapper.Setup(m => m.Map<IEnumerable<PostDto>>(It.IsAny<IEnumerable<Post>>()))
            .Returns(postDtos);

        var result = await _tagService.GetTagPage(id, page);

        result.Should().Be(tagDto);
        result?.Posts.Should().BeEquivalentTo(postDtos);
    }

    [Fact]
    public async void GetTagPage_Should_When_TagNotFound_Should_Return_Null()
    {
        var page = 10;
        var id = Guid.NewGuid();
        _mockRepository.Setup(r => r.Get(It.IsAny<Guid>()))
            .ReturnsAsync(()=> null);
        
        var result = await _tagService.GetTagPage(id, page);

        result.Should().BeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(687906)]
    public async void GetPageCount_Should_Returns_CorrectPageCount_ForGivenTotalPosts(int count)
    {
        var id = Guid.NewGuid();
        _mockRepository.Setup(r => r.GetPostCount(id))
            .ReturnsAsync(count);

        var result = await _tagService.GetPageCount(id);

        var expectedResult = (int)Math.Ceiling(count / (float)TagService.PageSize);
        Assert.Equal(result, expectedResult);
    }

    [Fact]
    public async void GetPageCount_Should_Call_Repository_GetCount()
    {
        var id = Guid.NewGuid();

        await _tagService.GetPageCount(id);
        _mockRepository.Verify(r => r.GetPostCount(id), Times.Once);
    }
}
