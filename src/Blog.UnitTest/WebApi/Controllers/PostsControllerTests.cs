using Blog.Application;
using Blog.WebApi.Controllers;
using Blog.WebApi.Models.Input;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Blog.UnitTest.WebApi.Controllers
{
    public class PostsControllerTests
    {
        private readonly Mock<IPostService> _mockPostService;
        private readonly PostsController _controller;

        public PostsControllerTests()
        {
            _mockPostService = new();
            _controller = new PostsController(_mockPostService.Object);
        }

        [Fact]
        public async Task Get_ReturnsOkWithPosts()
        {
            // Arrange
            var expectedPosts = new List<PostDto>
            {
                new(Guid.NewGuid(), "Title 1", "Content 1", [], []),
                new(Guid.NewGuid(), "Title 2", "Content 2", [], [])
            };

            _mockPostService.Setup(ps => ps.GetAll()).ReturnsAsync(expectedPosts);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var posts = okResult.Value.Should().BeAssignableTo<IEnumerable<PostDto>>().Subject;

            posts.Should().BeEquivalentTo(expectedPosts);
        }

        [Fact]
        public async Task Get_WithValidId_ReturnsOkWithPost()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var expectedPost = new PostDto(postId, "Title", "Content", [], []);

            _mockPostService.Setup(ps => ps.Get(postId)).ReturnsAsync(expectedPost);

            // Act
            var result = await _controller.Get(postId);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var post = okResult.Value.Should().BeAssignableTo<PostDto>().Subject;

            post.Should().BeEquivalentTo(expectedPost);
        }

        [Fact]
        public async Task Get_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var postId = Guid.NewGuid();

            _mockPostService.Setup(ps => ps.Get(postId)).ReturnsAsync(() => null);

            // Act
            var result = await _controller.Get(postId);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task Post_WithValidData_ReturnsCreated()
        {
            // Arrange
            var inputModel = new PostInputModel("Title", "Content");
            var expectedPostDto = new PostDto(Guid.NewGuid(), inputModel.Title, inputModel.Content, [], []);

            _mockPostService.Setup(ps => ps.Create(It.IsAny<PostDto>())).Callback<PostDto>(post =>
            {
                post.Id = expectedPostDto.Id;
                post.Title = expectedPostDto.Title;
                post.Content = expectedPostDto.Content;
            });

            // Act
            var result = await _controller.Post(inputModel);

            // Assert
            var createdAtActionResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdAtActionResult.ActionName.Should().Be(nameof(PostsController.Get));
            createdAtActionResult.RouteValues.Should().NotBeNull();
            createdAtActionResult.RouteValues?["id"].Should().Be(expectedPostDto.Id);
            createdAtActionResult.Value.Should().BeEquivalentTo(expectedPostDto);
        }

        [Fact]
        public async Task Post_WithInvalidInputModel_ReturnsBadRequest()
        {
            // Arrange
            var invalidInputModel = new PostInputModel("", "");
            _controller.ModelState.AddModelError("Title", "The Title field is required.");
            _controller.ModelState.AddModelError("Content", "The Content field is required.");

            // Act
            var result = await _controller.Post(invalidInputModel);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Put_WithValidIdAndValidInputModel_ReturnsNoContent()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var inputModel = new PostInputModel ("Updated Title", "Updated Content");
            var postDto = new PostDto (postId, "Original Title", "Original Content", [], []);

            _mockPostService.Setup(ps => ps.Get(postId)).ReturnsAsync(postDto);

            // Act
            var result = await _controller.Put(postId, inputModel);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Put_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var inputModel = new PostInputModel ("Updated Title", "Updated Content");

            _mockPostService.Setup(ps => ps.Get(postId)).ReturnsAsync(() => null);

            // Act
            var result = await _controller.Put(postId, inputModel);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task Put_WithInvalidInputModel_ReturnsBadRequest()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var invalidInputModel = new PostInputModel("", "");
            _controller.ModelState.AddModelError("Title", "The Title field is required.");
            _controller.ModelState.AddModelError("Content", "The Content field is required.");

            // Act
            var result = await _controller.Put(postId, invalidInputModel);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Delete_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var postDto = new PostDto(postId, "Title", "Content", [], []);

            _mockPostService.Setup(ps => ps.Get(postId)).ReturnsAsync(postDto);

            // Act
            var result = await _controller.Delete(postId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Delete_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var postId = Guid.NewGuid();

            _mockPostService.Setup(ps => ps.Get(postId)).ReturnsAsync(() => null);

            // Act
            var result = await _controller.Delete(postId);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}
