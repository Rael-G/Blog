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
        private readonly Comment _comment;

        public CommentServiceTests()
        {
            _mockRepository = new Mock<ICommentRepository>();
            _mockMapper = new Mock<IMapper>();
            _commentService = new CommentService(_mockRepository.Object, _mockMapper.Object);

            _post = new(Guid.NewGuid(), "Title", "Content");
            _comment = new(Guid.NewGuid(), "Author", "Content", _post.Id);
        }

        [Fact]
        public void Create_Should_Call_Repository_Create()
        {
            var commentDto = new CommentDto(Guid.NewGuid(), "Author", "Content", Guid.NewGuid());

            _mockMapper.Setup(m => m.Map<Comment>(It.IsAny<CommentDto>()))
                .Returns(_comment);

            _commentService.Create(commentDto);

            _mockRepository.Verify(m => m.Create(_comment), Times.Once);
        }

        [Fact]
        public void Update_Should_Call_Repository_Create()
        {
            var commentDto = new CommentDto(Guid.NewGuid(), "Author", "Content", Guid.NewGuid());

            _mockMapper.Setup(m => m.Map<Comment>(It.IsAny<CommentDto>()))
                .Returns(_comment);

            _commentService.Update(commentDto);

            _mockRepository.Verify(m => m.Update(_comment), Times.Once);
        }

        [Fact]
        public void Update_Should_UpdateTime()
        {
            var commentDto = new CommentDto(Guid.NewGuid(), "Author", "Content", Guid.NewGuid());
            var comment = new Comment(Guid.NewGuid(), commentDto.Author, commentDto.Content, Guid.NewGuid());

            _mockMapper.Setup(m => m.Map<Comment>(It.IsAny<CommentDto>()))
                .Returns(comment);

            var before = DateTime.UtcNow;
            _commentService.Update(commentDto);
            var after = DateTime.UtcNow;

            comment.ModifiedTime.Should().BeAfter(before);
            comment.ModifiedTime.Should().BeBefore(after);
        }

        [Fact]
        public void Delete_Should_Call_Repository_Delete()
        {
            var commentDto = new CommentDto(Guid.NewGuid(), "Author", "Content", Guid.NewGuid());

            _commentService.Delete(commentDto);

            _mockRepository.Verify(m => m.Delete(It.IsAny<Comment>()), Times.Once);
        }

        [Fact]
        public async Task Get_Should_Call_Repository_Get_And_Map_To_Dto()
        {
            var commentDto = new CommentDto(_comment.Id, _comment.Author, _comment.Content, _comment.PostId);

            _mockRepository.Setup(m => m.Get(It.IsAny<Guid>()))
                .ReturnsAsync(_comment);
            _mockMapper.Setup(m => m.Map<CommentDto>(_comment))
                .Returns(commentDto);

            var result = await _commentService.Get(_comment.Id);

            result.Should().BeEquivalentTo(commentDto);
            _mockRepository.Verify(m => m.Get(_comment.Id), Times.Once);        }

        [Fact]
        public async Task GetAll_Should_Call_Repository_GetAllByPostId_And_Map_To_DtoCollection()
        {
            var comments = new List<Comment>();
            var commentDtos = new List<CommentDto>();

            _mockRepository.Setup(m => m.GetAllByPostId(It.IsAny<Guid>())).ReturnsAsync(comments);
            _mockMapper.Setup(m => m.Map<IEnumerable<CommentDto>>(comments))
                .Returns(commentDtos);

            var result = await _commentService.GetAll(_post.Id);

            result.Should().BeEquivalentTo(commentDtos);
            _mockRepository.Verify(m => m.GetAllByPostId(_post.Id), Times.Once);
        }
    }
}