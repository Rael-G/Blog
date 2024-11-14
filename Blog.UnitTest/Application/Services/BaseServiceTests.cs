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
    public async void Create_Should_Call_Repository_CreateAndCommit()
    {
        var dto = new TestDto();

        _mockMapper.Setup(r => r.Map<TestEntity>(It.IsAny<TestDto>()))
            .Returns(_entity);

        await _testService.Create(dto);

        _mockRepository.Verify(r => r.Create(_entity), Times.Once);
        _mockRepository.Verify(r => r.Commit(), Times.Once);

    }

    [Fact]
    public async void Update_Should_Call_Repository_UpdateAndCommit()
    {
        var dto = new TestDto();

        _mockMapper.Setup(r => r.Map<TestEntity>(It.IsAny<TestDto>()))
            .Returns(_entity);

        await _testService.Update(dto);

        _mockRepository.Verify(r => r.Update(_entity), Times.Once);
        _mockRepository.Verify(r => r.Commit(), Times.Once);
    }

    [Fact]
    public async void Delete_Should_Call_Repository_DeleteAndCommit()
    {
        var dto = new TestDto();

        await _testService.Delete(dto);

        _mockRepository.Verify(r => r.Delete(It.IsAny<TestEntity>()), Times.Once);
        _mockRepository.Verify(r => r.Commit(), Times.Once);
    }

    [Fact]
    public async Task Get_Should_Call_Repository_Get_And_Map_To_Dto()
    {
        var dto = new TestDto();

        _mockRepository.Setup(r => r.Get(It.IsAny<Guid>()))
            .ReturnsAsync(_entity);
        _mockMapper.Setup(r => r.Map<TestDto>(_entity))
            .Returns(dto);

        var result = await _testService.Get(_entity.Id);

        result.Should().BeEquivalentTo(dto);
        _mockRepository.Verify(r => r.Get(_entity.Id), Times.Once);
    }

    [Fact]
    public async Task GetAll_Should_Call_Repository_GetAll_And_Map_To_DtoCollection()
    {
        var posts = new List<TestEntity>();
        var postDtos = new List<TestDto>();

        _mockRepository.Setup(r => r.GetAll()).ReturnsAsync(posts);
        _mockMapper.Setup(r => r.Map<IEnumerable<TestDto>>(posts))
            .Returns(postDtos);

        var result = await _testService.GetAll();

        result.Should().BeEquivalentTo(postDtos);
        _mockRepository.Verify(r => r.GetAll(), Times.Once);
    }

}