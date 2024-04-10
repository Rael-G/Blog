using Blog.Application;
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
    public async Task Get_WithValidId_ReturnsOkWithTags()
    {
        var tags = new List<TagDto>{ new TagDto{ Id = Guid.NewGuid() }, new TagDto{ Id = Guid.NewGuid() } };
        _mockService.Setup(s => s.GetTags(It.IsAny<Guid>())).ReturnsAsync(tags);

        var result = await _controller.GetTags(_postDto.Id);

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var resultValue = okResult.Value.Should().BeAssignableTo<IEnumerable<TagDto>>().Subject;
        resultValue.Should().BeEquivalentTo(tags);
    }

    [Fact]
    public async Task Get_WithInvalidId_ReturnsNotFound()
    {
        _mockService.Setup(s => s.Get(It.IsAny<Guid>())).ReturnsAsync(() => null);

        var result = await _controller.Get(_postDto.Id);

        result.Should().BeOfType<NotFoundObjectResult>();
    }

}
