using AutoMapper;
using Blog.Application;
using Blog.Domain;
using FluentAssertions;
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

            [Fact]
        public async Task GetTags_Should_Call_Repository_Get()
        {
            var tags = new List<TagDto>();

            _mockRepository.Setup(r => r.Get(It.IsAny<Guid>()))
                .ReturnsAsync(_post);
            _mockMapper.Setup(r => r.Map<IEnumerable<TagDto>>(It.IsAny<IEnumerable<Tag>>()))
                .Returns(tags);

            var result = await _postService.GetTags(_post.Id);

            _mockRepository.Verify(r => r.Get(_post.Id), Times.Once);
        }

        [Fact]
        public async Task GetTags_WhenPostExists_ShouldReturn_TagsDto()
        {
            var tags = new List<TagDto>();

            _mockRepository.Setup(r => r.Get(It.IsAny<Guid>()))
                .ReturnsAsync(_post);
            _mockMapper.Setup(r => r.Map<IEnumerable<TagDto>>(It.IsAny<IEnumerable<Tag>>()))
                .Returns(tags);

            var result = await _postService.GetTags(_post.Id);

            result.Should().BeSameAs(tags);
        }

        [Fact]
        public async Task GetTags_WhenPostIsNull_ShouldReturn_Null()
        {
            _mockRepository.Setup(r => r.Get(It.IsAny<Guid>()))
                .ReturnsAsync(() => null);

            var result = await _postService.GetTags(_post.Id);

            result.Should().BeNull();
        }
}