using Blog.Application;
using Blog.WebApi;
using Blog.WebApi.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Blog.UnitTest.WebApi.Controllers;

public class PostsControllerTests
{
    private readonly Mock<IPostService> _mockService;
    private readonly PostsController _controller;

    public PostDto _postDto;
    public PostsControllerTests()
    {
        _mockService = new Mock<IPostService>();
        _controller = new PostsController(_mockService.Object);

        _postDto = new PostDto { Id = Guid.NewGuid() };
    }

    [Fact]
    public async Task GetPage_ReturnsOkWithDtos()
    {
        var page = 10;
        var expectedDtos = new List<PostDto>
        {
            new PostDto{ Id = Guid.NewGuid()},
            new PostDto{ Id = Guid.NewGuid()}
        };
        _mockService.Setup(s => s.GetPage(It.IsAny<int>()))
            .ReturnsAsync(expectedDtos);

        var result = await _controller.GetPage(page);

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var posts = okResult.Value.Should().BeAssignableTo<IEnumerable<PostOutputModel>>().Subject;

        posts.Should().BeEquivalentTo(PostOutputModel.MapRange(expectedDtos));
    }

    [Fact]
    public async Task GetPageCount_ReturnsOkWithCount()
    {
        var count = 19;
        _mockService.Setup(s => s.GetPageCount())
            .ReturnsAsync(count);

        var result = await _controller.GetPageCount();

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var resultCount = okResult.Value.Should().Be(count);
    }
}
