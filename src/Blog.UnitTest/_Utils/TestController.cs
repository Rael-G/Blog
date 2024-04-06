using Blog.Application;
using Blog.WebApi;
using Microsoft.AspNetCore.Mvc;

namespace Blog.UnitTest;

public class TestController(IBaseService<TestDto> testService)
    : BaseController<TestDto>(testService)
{
    public new async Task<IActionResult> Get(Guid id)
        => await base.Get(id);

    public new async Task<IActionResult> GetAll()
        => await base.GetAll();

    public new async Task<IActionResult> Post([FromBody] IInputModel<TestDto> input)
        => await base.Post(input);

    public new async Task<IActionResult> Put(Guid id, [FromBody] IInputModel<TestDto> input)
        => await base.Put(id, input);

    public new async Task<IActionResult> Delete(Guid id)
        => await base.Delete(id);
}
