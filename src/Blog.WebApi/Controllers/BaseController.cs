using Blog.Application;
using Microsoft.AspNetCore.Mvc;

namespace Blog.WebApi;

[Route("api/[controller]")]
[ApiController]
public abstract class BaseController<TDto>(IBaseService<TDto> service)
    : ControllerBase
    where TDto : IDto
{
    protected readonly IBaseService<TDto> Service = service;

    /// <summary>
    /// Retrieves all <see cref="TDto">.
    /// </summary>
    /// <returns>Returns a list of all <see cref="TDto">.</returns>
    [HttpGet]
    [ProducesResponseType(200)] // OK
    protected async Task<IActionResult> GetAll()
        => Ok(await Service.GetAll());

    /// <summary>
    /// Retrieves a specific <see cref="TDto"> post by its ID.
    /// </summary>
    /// <param name="id">The ID of the <see cref="TDto"> to retrieve.</param>
    /// <returns>Returns the <see cref="TDto"> if found, otherwise returns a 404 Not Found.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(200)] // OK
    [ProducesResponseType(404)] // Not Found
    protected async Task<IActionResult> Get(Guid id)
    {
        var entity = await Service.Get(id);

        if (entity is null)
            return NotFound(id);

        return Ok(entity);
    }

    /// <summary>
    /// Creates a new <see cref="TDto">.
    /// </summary>
    /// <param name="input">The input model containing data for the new <see cref="TDto">.</param>
    /// <returns>Returns the newly created <see cref="TDto">.</returns>
    [HttpPost]
    [ProducesResponseType(201)] // Created
    [ProducesResponseType(400)] // Bad Request
    protected async Task<IActionResult> Post([FromBody] IInputModel<TDto> input)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var entity = input.InputToDto();
        try
        {
            Service.Create(entity);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }

        await Service.Commit();
        return CreatedAtAction(nameof(Get), new { entity.Id }, entity);
    }

    /// <summary>
    /// Updates an existing <see cref="TDto">.
    /// </summary>
    /// <param name="id">The ID of the <see cref="TDto"> to update.</param>
    /// <param name="input">The input model containing updated data for the <see cref="TDto">.</param>
    /// <returns>Returns 204 No Content if successful, otherwise returns a 404 Not Found or 400 Bad Request.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(204)] // No Content
    [ProducesResponseType(400)] // Bad Request
    [ProducesResponseType(404)] // Not Found
    protected async Task<IActionResult> Put(Guid id, [FromBody] IInputModel<TDto> input)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var entity = await Service.Get(id);
        if (entity is null)
            return NotFound(id);

        input.InputToDto(entity);
        try
        {
            Service.Update(entity);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }

        await Service.Commit();
        return NoContent();
    }

    /// <summary>
    /// Deletes a <see cref="TDto"> by its ID.
    /// </summary>
    /// <param name="id">The ID of the <see cref="TDto"> to delete.</param>
    /// <returns>Returns 204 No Content if successful, otherwise returns a 404 Not Found.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(204)] // No Content
    [ProducesResponseType(404)] // Not Found
    protected async Task<IActionResult> Delete(Guid id)
    {
        var post = await Service.Get(id);

        if (post is null)
            return NotFound(id);

        Service.Delete(post);

        await Service.Commit();
        return NoContent();
    }
}
