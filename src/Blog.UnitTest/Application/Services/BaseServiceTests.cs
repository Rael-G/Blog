using AutoMapper;
using Blog.Application;
using Blog.Domain;
using Moq;
using FluentAssertions;

namespace Blog.UnitTest.Application.Services;

public class BaseServiceTests
{
    private readonly Mock<IBaseRepository<TestEntity>> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly BaseService<TestDto, TestEntity> _testService;

    private readonly TestEntity _entity;

    public BaseServiceTests()
    {
        _mockRepository = new Mock<IBaseRepository<TestEntity>>();
        _mockMapper = new Mock<IMapper>();
        _testService = new TestService(_mockRepository.Object, _mockMapper.Object);

        _entity = new TestEntity(Guid.NewGuid());
    }

    [Fact]
    public void Create_Should_Call_Repository_Create()
    {
        var dto = new TestDto();

        _mockMapper.Setup(m => m.Map<TestEntity>(It.IsAny<TestDto>()))
            .Returns(_entity);

        _testService.Create(dto);

        _mockRepository.Verify(m => m.Create(_entity), Times.Once);
    }

    [Fact]
    public void Update_Should_Call_Repository_Update()
    {
        var dto = new TestDto();

        _mockMapper.Setup(m => m.Map<TestEntity>(It.IsAny<TestDto>()))
            .Returns(_entity);

        _testService.Update(dto);

        _mockRepository.Verify(m => m.Update(_entity), Times.Once);
    }

    [Fact]
    public void Delete_Should_Call_Repository_Delete()
    {
        var dto = new TestDto();

        _testService.Delete(dto);

        _mockRepository.Verify(m => m.Delete(It.IsAny<TestEntity>()), Times.Once);
    }

    [Fact]
    public async Task Get_Should_Call_Repository_Get_And_Map_To_Dto()
    {
        var dto = new TestDto();

        _mockRepository.Setup(m => m.Get(It.IsAny<Guid>()))
            .ReturnsAsync(_entity);
        _mockMapper.Setup(m => m.Map<TestDto>(_entity))
            .Returns(dto);

        var result = await _testService.Get(_entity.Id);

        result.Should().BeEquivalentTo(dto);
        _mockRepository.Verify(m => m.Get(_entity.Id), Times.Once);
    }

    [Fact]
    public async Task GetAll_Should_Call_Repository_GetAll_And_Map_To_DtoCollection()
    {
        var posts = new List<TestEntity>();
        var postDtos = new List<TestDto>();

        _mockRepository.Setup(m => m.GetAll()).ReturnsAsync(posts);
        _mockMapper.Setup(m => m.Map<IEnumerable<TestDto>>(posts))
            .Returns(postDtos);

        var result = await _testService.GetAll();

        result.Should().BeEquivalentTo(postDtos);
        _mockRepository.Verify(m => m.GetAll(), Times.Once);
    }

    [Fact]
    public async void Commit_Should_Call_Repository_Commit()
    {
        await _testService.Commit();

        _mockRepository.Verify(m => m.Commit(), Times.Once);
    }
}