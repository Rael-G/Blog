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
                new() { Id = Guid.NewGuid(), Title = "Title 1", Content = "Content 1" },
                new() { Id = Guid.NewGuid(), Title = "Title 2", Content = "Content 2" }
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
            var expectedPost = new PostDto { Id = postId, Title = "Title", Content = "Content" };

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
            var inputModel = new PostInputModel { Title = "Title", Content = "Content" };
            var expectedPostDto = new PostDto { Id = Guid.NewGuid(), Title = inputModel.Title, Content = inputModel.Content, Comments = [] };

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
            createdAtActionResult.RouteValues["id"].Should().Be(expectedPostDto.Id);
            createdAtActionResult.Value.Should().BeEquivalentTo(expectedPostDto);
        }

        [Fact]
        public async Task Post_WithInvalidInputModel_ReturnsBadRequest()
        {
            // Arrange
            var inputModel = new PostInputModel { }; // Invalid input model
            _controller.ModelState.AddModelError("Title", "The Title field is required.");
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
            var postId = Guid.NewGuid();
            var inputModel = new PostInputModel { Title = "Updated Title", Content = "Updated Content" };
            var postDto = new PostDto { Id = postId, Title = "Original Title", Content = "Original Content" };

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
            var inputModel = new PostInputModel { Title = "Updated Title", Content = "Updated Content" };

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
            var inputModel = new PostInputModel { }; // Invalid input model
            _controller.ModelState.AddModelError("Title", "The Title field is required.");
            _controller.ModelState.AddModelError("Content", "The Content field is required.");

            // Act
            var result = await _controller.Put(postId, inputModel);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Delete_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var postDto = new PostDto { Id = postId };

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
