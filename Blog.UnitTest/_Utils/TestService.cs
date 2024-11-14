using AutoMapper;
using Blog.Application;
using Blog.Domain;

namespace Blog.UnitTest;

public class TestService(IBaseRepository<TestEntity> repository, IMapper mapper)
    : BaseService<TestDto, TestEntity>(repository, mapper)
{ }