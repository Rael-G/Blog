using Blog.Application;
using Blog.WebApi;
using Blog.WebApi.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Blog.UnitTest.WebApi.Controllers;

public class TagsControllerTests
{
    private readonly Mock<ITagService> _mockService;
    private readonly TagsController _controller;

    public TagDto _tagDto;
    public TagsControllerTests()
    {
        _mockService = new Mock<ITagService>();
        _controller = new TagsController(_mockService.Object);

        _tagDto = new TagDto { Id = Guid.NewGuid() };
    }

    [Fact]
    public async Task GetPage_ReturnsOkWithDto()
    {
        var page = 10;
        _mockService.Setup(s => s.GetTagPage(It.IsAny<Guid>(), It.IsAny<int>()))
            .ReturnsAsync(_tagDto);

        var result = await _controller.GetPage(_tagDto.Id, page);

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().Be(_tagDto);
    }

    [Fact]
    public async Task GetPage_WhenTagNull_ReturnsNotFoundWithId()
    {
        var page = 10;
        _mockService.Setup(s => s.GetTagPage(It.IsAny<Guid>(), It.IsAny<int>()))
            .ReturnsAsync(() => null);

        var result = await _controller.GetPage(_tagDto.Id, page);

        var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        var resultId = notFoundResult.Value;
        resultId.Should().BeEquivalentTo(new { _tagDto.Id });
    }

    [Fact]
    public async Task GetPageCount_WhenTagNull_ReturnsNotFoundWithId()
    {
        _mockService.Setup(s => s.Get(It.IsAny<Guid>()))
            .ReturnsAsync(() => null);

        var result = await _controller.GetPageCount(_tagDto.Id);

        var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        notFoundResult.Value.Should().BeEquivalentTo(new { _tagDto.Id });
    }

    [Fact]
    public async Task GetPageCount_ReturnsOkWithCount()
    {
        var count = 19;
        _mockService.Setup(s => s.Get(It.IsAny<Guid>()))
            .ReturnsAsync(_tagDto);
        _mockService.Setup(s => s.GetPageCount(It.IsAny<Guid>()))
            .ReturnsAsync(count);

        var result = await _controller.GetPageCount(_tagDto.Id);

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().Be(count);

    }
}
