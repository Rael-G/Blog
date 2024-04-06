using AutoMapper;
using Blog.Application;
using Blog.Domain;
using Moq;
using FluentAssertions;

namespace Blog.UnitTest.Application.Services
{
    public class CommentServiceTests
    {
        private readonly Mock<ICommentRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CommentService _commentService;

        private readonly Post _post;

        public CommentServiceTests()
        {
            _mockRepository = new Mock<ICommentRepository>();
            _mockMapper = new Mock<IMapper>();
            _commentService = new CommentService(_mockRepository.Object, _mockMapper.Object);

            _post = new(Guid.NewGuid(), "Title", "Content");
        }

        [Fact]
        public async Task GetAll_Should_Call_Repository_GetAllByPostId_And_Map_To_DtoCollection()
        {
            var comments = new List<Comment>();
            var commentDtos = new List<CommentDto>();

            _mockRepository.Setup(r => r.GetAllByPostId(It.IsAny<Guid>()))
                .ReturnsAsync(comments);
            _mockMapper.Setup(r => r.Map<IEnumerable<CommentDto>>(comments))
                .Returns(commentDtos);

            var result = await _commentService.GetAll(_post.Id);

            _mockRepository.Verify(r => r.GetAllByPostId(_post.Id), Times.Once);
        }

        [Fact]
        public async Task GetAll_Should_Map_To_DtoCollection()
        {
            var comments = new List<Comment>();
            var commentDtos = new List<CommentDto>();

            _mockRepository.Setup(r => r.GetAllByPostId(It.IsAny<Guid>()))
                .ReturnsAsync(comments);
            _mockMapper.Setup(r => r.Map<IEnumerable<CommentDto>>(comments))
                .Returns(commentDtos);

            var result = await _commentService.GetAll(_post.Id);

            result.Should().BeEquivalentTo(commentDtos);
        }
    }
}