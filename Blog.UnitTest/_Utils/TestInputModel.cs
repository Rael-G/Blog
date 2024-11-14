using Blog.WebApi;

namespace Blog.UnitTest;

public class TestInputModel : IInputModel<TestDto>
{
    public TestDto InputToDto()
        => new() { Id = Guid.NewGuid() };

    public void InputToDto(TestDto postDto)
    { }
}