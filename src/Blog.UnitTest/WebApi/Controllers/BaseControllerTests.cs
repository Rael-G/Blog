using Blog.Application;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Blog.UnitTest.WebApi.Controllers;

public class BaseControllerTests
{
    private readonly Mock<IBaseService<TestDto>> _mockService;
    private readonly TestController _controller;

    private readonly Guid _id = Guid.NewGuid();
    private readonly TestDto _dto;
    private readonly TestInputModel _inputModel = new TestInputModel();

    public BaseControllerTests()
    {
        _mockService = new Mock<IBaseService<TestDto>>();
        _controller = new TestController(_mockService.Object);

        _dto = new TestDto { Id = _id };
    }

    [Fact]
    public async Task GetAll_ReturnsOkWithDtos()
    {
        var expectedDtos = new List<TestDto>
        {
            new TestDto{ Id = Guid.NewGuid()},
            new TestDto{ Id = Guid.NewGuid()}
        };
        _mockService.Setup(s => s.GetAll())
            .ReturnsAsync(expectedDtos);

        var result = await _controller.GetAll();

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var posts = okResult.Value.Should().BeAssignableTo<IEnumerable<TestDto>>().Subject;

        posts.Should().BeEquivalentTo(expectedDtos);
    }

    [Fact]
    public async Task Get_WithValidId_ReturnsOkWithDto()
    {
        _mockService.Setup(s => s.Get(_id)).ReturnsAsync(_dto);

        var result = await _controller.Get(_id);

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var post = okResult.Value.Should().BeAssignableTo<TestDto>().Subject;

        post.Should().BeEquivalentTo(_dto);
    }

    [Fact]
    public async Task Get_WithInvalidId_ReturnsNotFound()
    {
        _mockService.Setup(s => s.Get(_id)).ReturnsAsync(() => null);

        var result = await _controller.Get(_id);

        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Post_WithValidData_ReturnsCreated()
    {
        _mockService.Setup(s => s.Create(It.IsAny<TestDto>())).Callback<TestDto>(post =>
        {
            post.Id = _dto.Id;
        });

        var result = await _controller.Post(_inputModel);

        var createdAtActionResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdAtActionResult.ActionName.Should().Be(nameof(TestController.Get));
        createdAtActionResult.RouteValues.Should().NotBeNull();
        createdAtActionResult.RouteValues?["id"].Should().Be(_dto.Id);
        createdAtActionResult.Value.Should().BeEquivalentTo(_dto);
    }

    [Fact]
    public async Task Post_WithInvalidInputModel_ReturnsBadRequest()
    {
        _controller.ModelState.AddModelError("Id", "The Id field is required.");

        var result = await _controller.Post(_inputModel);

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Put_WithValidIdAndValidInputModel_ReturnsNoContent()
    {
        _mockService.Setup(s => s.Get(_id)).ReturnsAsync(_dto);

        var result = await _controller.Put(_id, _inputModel);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Put_WithInvalidId_ReturnsNotFound()
    {
        _mockService.Setup(s => s.Get(_id)).ReturnsAsync(() => null);

        var result = await _controller.Put(_id, _inputModel);

        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Put_WithInvalidInputModel_ReturnsBadRequest()
    {
        _controller.ModelState.AddModelError("Id", "The Id field is required.");

        var result = await _controller.Put(_id, _inputModel);

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Delete_WithValidId_ReturnsNoContent()
    {
        _mockService.Setup(s => s.Get(_id)).ReturnsAsync(_dto);

        var result = await _controller.Delete(_id);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Delete_WithInvalidId_ReturnsNotFound()
    {
        _mockService.Setup(s => s.Get(_id)).ReturnsAsync(() => null);

        var result = await _controller.Delete(_id);

        result.Should().BeOfType<NotFoundObjectResult>();
    }
}