using Blog.Application;
using Blog.WebApi.Controllers;
using Blog.WebApi.Models.Input;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Blog.UnitTest.WebApi.Controllers;

public class CommentsControllerTests
{
    private readonly Mock<IPostService> _mockPostService;
    private readonly Mock<ICommentService> _mockCommentService;
    private readonly CommentsController _controller;
    private readonly CommentInputModel _inputModel;

    private readonly Guid _postId = Guid.NewGuid();
    public CommentsControllerTests()
    {
        _mockPostService = new();
        _mockCommentService = new();
        _controller = new(_mockCommentService.Object, _mockPostService.Object);

        _inputModel = new CommentInputModel() { Author = "Author", Content = "Content", PostId = _postId };
    }

    [Fact]
    public async Task GetAll_WithValidPostId_ReturnsOkWithComments()
    {
        var expectedComments = new List<CommentDto>
        {
            new(Guid.NewGuid(), "Author 1", "Comment 1", _postId),
            new(Guid.NewGuid(), "Author 2", "Comment 2", _postId)
        };
        var post = new PostDto(_postId, "Title", "Content", expectedComments, []);

        _mockPostService.Setup(ps => ps.Get(_postId)).ReturnsAsync(post);
        _mockCommentService.Setup(cs => cs.GetAll(_postId)).ReturnsAsync(expectedComments);

        var result = await _controller.GetAll(_postId);

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var comments = okResult.Value.Should().BeAssignableTo<IEnumerable<CommentDto>>().Subject;

        comments.Should().BeEquivalentTo(expectedComments);
    }

    [Fact]
    public async Task GetAll_WithInvalidPostId_ReturnsNotFound()
    {
        var post = new PostDto(_postId, "Title", "Content", [], []);
        _mockPostService.Setup(ps => ps.Get(_postId)).ReturnsAsync(() => null);

        var result = await _controller.GetAll(_postId);

        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Post_WithValidData_ReturnsCreated()
    {
        _mockPostService.Setup(ps => ps.Get(_postId)).ReturnsAsync(new PostDto(_postId, "Title", "Content", [], []));

        var result = await _controller.Post(_inputModel);

        var createdAtActionResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdAtActionResult.ActionName.Should().Be(nameof(CommentsController.Get));
        createdAtActionResult.RouteValues.Should().NotBeNull();
        createdAtActionResult.RouteValues?["id"].Should().Be(createdAtActionResult.Value.As<CommentDto>().Id);
        _mockCommentService.Verify(cs => cs.Commit(), Times.Once());

    }

    [Fact]
    public async Task Post_WithInvalidPostId_ReturnsNotFound()
    {
        _mockPostService.Setup(ps => ps.Get(_inputModel.PostId)).ReturnsAsync(() => null);

        var result = await _controller.Post(_inputModel);

        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Post_WithInvalidInputModel_ReturnsBadRequest()
    {
        _controller.ModelState.AddModelError("Author", "The Author field is required.");
        _controller.ModelState.AddModelError("Content", "The Content field is required.");

        var result = await _controller.Post(_inputModel);

        result.Should().BeOfType<BadRequestObjectResult>();
    }
}