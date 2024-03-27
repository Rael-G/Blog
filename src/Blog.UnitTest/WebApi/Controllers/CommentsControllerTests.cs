using Blog.Application;
using Blog.WebApi.Controllers;
using Blog.WebApi.Models.Input;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Blog.UnitTest.WebApi.Controllers
{
    public class CommentsControllerTests
    {
        private readonly Mock<IPostService> _mockPostService;
        private readonly Mock<ICommentService> _mockCommentService;
        private readonly CommentsController _controller;

        public CommentsControllerTests()
        {
            _mockPostService = new();
            _mockCommentService = new();
            _controller = new(_mockCommentService.Object, _mockPostService.Object);
        }

        [Fact]
        public async Task GetAll_WithValidPostId_ReturnsOkWithComments()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var expectedComments = new List<CommentDto>
            {
                new() { Id = Guid.NewGuid(), PostId = postId, Author = "Author 1", Content = "Comment 1" },
                new() { Id = Guid.NewGuid(), PostId = postId, Author = "Author 2", Content = "Comment 2" }
            };
            var post = new PostDto() { Id = postId, Title = "Title", Content = "Content", Comments = expectedComments };

            _mockPostService.Setup(ps => ps.Get(postId)).ReturnsAsync(post);
            _mockCommentService.Setup(cs => cs.GetAll(postId)).ReturnsAsync(expectedComments);

            // Act
            var result = await _controller.GetAll(postId);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var comments = okResult.Value.Should().BeAssignableTo<IEnumerable<CommentDto>>().Subject;

            comments.Should().BeEquivalentTo(expectedComments);
        }

        [Fact]
        public async Task GetAll_WithInvalidPostId_ReturnsNotFound()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var post = new PostDto() { Id = postId, Title = "Title", Content = "Content", Comments = [] };

            _mockPostService.Setup(ps => ps.Get(postId)).ReturnsAsync(() => null);

            var mockCommentService = new Mock<ICommentService>();

            // Act
            var result = await _controller.GetAll(postId);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task Get_WithValidId_ReturnsOkWithComment()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var expectedComment = new CommentDto { Id = commentId, Author = "Author 1", Content = "Comment 1" };

            _mockCommentService.Setup(cs => cs.Get(commentId)).ReturnsAsync(expectedComment);

            // Act
            var result = await _controller.Get(commentId);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var comment = okResult.Value.Should().BeAssignableTo<CommentDto>().Subject;

            comment.Should().BeEquivalentTo(expectedComment);
        }

        [Fact]
        public async Task Get_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var commentId = Guid.NewGuid();

            _mockCommentService.Setup(cs => cs.Get(commentId)).ReturnsAsync(() => null);

            // Act
            var result = await _controller.Get(commentId);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task Post_WithValidData_ReturnsCreated()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var inputModel = new CommentInputModel { Author = "Author", Content = "Content", PostId = postId };

            _mockPostService.Setup(ps => ps.Get(postId)).ReturnsAsync(new PostDto { Id = postId });

            // Act
            var result = await _controller.Post(inputModel);

            // Assert
            var createdAtActionResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdAtActionResult.ActionName.Should().Be(nameof(CommentsController.Get));
            createdAtActionResult.RouteValues["id"].Should().Be(createdAtActionResult.Value.As<CommentDto>().Id);
            _mockCommentService.Verify(cs => cs.Commit(), Times.Once());

        }

        [Fact]
        public async Task Post_WithInvalidPostId_ReturnsNotFound()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var inputModel = new CommentInputModel { Author = "Author", Content = "Content", PostId = postId };

            _mockPostService.Setup(ps => ps.Get(inputModel.PostId)).ReturnsAsync(() => null);

            // Act
            var result = await _controller.Post(inputModel);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task Post_WithInvalidInputModel_ReturnsBadRequest()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var inputModel = new CommentInputModel { Author = "invalid author", Content = "invalid content", PostId = postId}; // Invalid input model
            _controller.ModelState.AddModelError("Author", "The Author field is required.");
            _controller.ModelState.AddModelError("Content", "The Content field is required.");

            // Act
            var result = await _controller.Post(inputModel);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Put_WithValidIdAndValidInputModel_ReturnsNoContent()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var inputModel = new CommentInputModel { Author = "Updated Author", Content = "Updated Content", PostId = postId };
            var commentDto = new CommentDto { Id = commentId, Author = "Original Author", Content = "Original Content", PostId = postId };

            _mockCommentService.Setup(cs => cs.Get(commentId)).ReturnsAsync(commentDto);

            // Act
            var result = await _controller.Put(commentId, inputModel);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _mockCommentService.Verify(cs => cs.Commit(), Times.Once());

        }

        [Fact]
        public async Task Put_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var inputModel = new CommentInputModel { Author = "Updated Author", Content = "Updated Content", PostId = Guid.NewGuid() };

            _mockCommentService.Setup(cs => cs.Get(commentId)).ReturnsAsync(() => null);

            // Act
            var result = await _controller.Put(commentId, inputModel);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task Put_WithInvalidInputModel_ReturnsBadRequest()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var inputModel = new CommentInputModel { Author = "Invalid Author", Content = "Invalid Content", PostId = Guid.NewGuid() }; // Invalid input model
            _controller.ModelState.AddModelError("Author", "The Author field is required.");
            _controller.ModelState.AddModelError("Content", "The Content field is required.");

            // Act
            var result = await _controller.Put(commentId, inputModel);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Delete_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var commentDto = new CommentDto { Id = commentId };

            _mockCommentService.Setup(cs => cs.Get(commentId)).ReturnsAsync(commentDto);
            _mockCommentService.Setup(cs => cs.Commit()).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(commentId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _mockCommentService.Verify(cs => cs.Commit(), Times.Once());
        }

        [Fact]
        public async Task Delete_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var commentId = Guid.NewGuid();

            _mockCommentService.Setup(cs => cs.Get(commentId)).ReturnsAsync(() => null);

            // Act
            var result = await _controller.Delete(commentId);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}
