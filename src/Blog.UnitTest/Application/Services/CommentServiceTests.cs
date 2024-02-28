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

        private readonly Comment _comment = new(Guid.NewGuid(), "Author", "Content");
        
        public CommentServiceTests()
        {
            _mockRepository = new Mock<ICommentRepository>();
            _mockMapper = new Mock<IMapper>();
            _commentService = new CommentService(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public void Create_Should_Call_Validate_And_Repository_Create()
        {
            var commentDto = new CommentDto()
            { Author = "Author", Content = "Content" };

            _mockMapper.Setup(m => m.Map<Comment>(It.IsAny<CommentDto>()))
                .Returns(_comment);

            _commentService.Create(commentDto);

            _mockRepository.Verify(m => m.Create(_comment), Times.Once);
        }

        [Fact]
        public void Update_Should_Call_Validate_And_Repository_Update()
        {
            var commentDto = new CommentDto()
            { Id = _comment.Id, Author = "Other Author", Content = "Another Content" };

            _mockMapper.Setup(m => m.Map<Comment>(It.IsAny<CommentDto>()))
                .Returns(_comment);

            _commentService.Update(commentDto);

            _mockRepository.Verify(m => m.Update(_comment), Times.Once);
        }

        [Fact]
        public void Delete_Should_Call_Repository_Delete()
        {
            var commentDto = new CommentDto();

            _commentService.Delete(commentDto);

            _mockRepository.Verify(m => m.Delete(It.IsAny<Comment>()), Times.Once);
        }

        [Fact]
        public async Task Get_Should_Call_Repository_Get_And_Map_To_Dto()
        {
            var commentDto = new CommentDto()
            { Id = _comment.Id, Author = _comment.Author, Content = _comment.Content } ;

            _mockRepository.Setup(m => m.Get(It.IsAny<Guid>()))
                .ReturnsAsync(_comment);
            _mockMapper.Setup(m => m.Map<CommentDto>(_comment))
                .Returns(commentDto);

            var result = await _commentService.Get(_comment.Id);

            result.Should().BeEquivalentTo(commentDto);
            _mockRepository.Verify(m => m.Get(_comment.Id), Times.Once);        }

        [Fact]
        public async Task GetAll_Should_Call_Repository_GetAll_And_Map_To_DtoCollection()
        {
            var comments = new List<Comment>();
            var commentDtos = new List<CommentDto>();

            _mockRepository.Setup(m => m.GetAll()).ReturnsAsync(comments);
            _mockMapper.Setup(m => m.Map<IEnumerable<CommentDto>>(comments))
                .Returns(commentDtos);

            var result = await _commentService.GetAll();

            result.Should().BeEquivalentTo(commentDtos);
            _mockRepository.Verify(m => m.GetAll(), Times.Once);
        }
    }
}