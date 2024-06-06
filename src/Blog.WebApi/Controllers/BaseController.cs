using Blog.Application;
using Blog.Domain;
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
    /// Retrieves all TEntities.
    /// </summary>
    /// <returns>Returns a list of all TEntities.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    protected async Task<IActionResult> GetAll()
        => Ok(await Service.GetAll());

    /// <summary>
    /// Retrieves a specific TEntity post by its ID.
    /// </summary>
    /// <param name="id">The ID of the TEntity to retrieve.</param>
    /// <returns>Returns the TEntity if found, otherwise returns a 404 Not Found.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    protected async Task<IActionResult> Get(Guid id)
    {
        var entity = await Service.Get(id);

        if (entity is null)
            return NotFound(new {Id = id});

        return Ok(entity);
    }

    /// <summary>
    /// Creates a new TEntity.
    /// </summary>
    /// <param name="input">The input model containing data for the new TEntity.</param>
    /// <returns>Returns the newly created Post.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    protected async Task<IActionResult> Post([FromBody] IInputModel<TDto> input)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var entity = input.InputToDto();
        try
        {
            await Service.Create(entity);
        }
        catch (DomainException ex)
        {
            return BadRequest(new { ex.Message });
        }

        return CreatedAtAction(nameof(Get), new { entity.Id }, entity);
    }

    /// <summary>
    /// Updates an existing TEntity.
    /// </summary>
    /// <param name="id">The ID of the TEntity to update.</param>
    /// <param name="input">The input model containing updated data for the TEntity.</param>
    /// <returns>Returns 204 No Content if successful, otherwise returns a 404 Not Found or 400 Bad Request.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    protected async Task<IActionResult> Put(Guid id, [FromBody] IInputModel<TDto> input)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var entity = await Service.Get(id);
        if (entity is null)
            return NotFound(new {Id = id});

        input.InputToDto(entity);
        try
        {
            await Service.Update(entity);
        }
        catch (DomainException ex)
        {
            return BadRequest(new { ex.Message });
        }

        return NoContent();
    }

    /// <summary>
    /// Deletes a TEntity by its ID.
    /// </summary>
    /// <param name="id">The ID of the TEntity to delete.</param>
    /// <returns>Returns 204 No Content if successful, otherwise returns a 404 Not Found.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    protected async Task<IActionResult> Delete(Guid id)
    {
        var post = await Service.Get(id);

        if (post is null)
            return NotFound(new {Id = id});

        await Service.Delete(post);

        return NoContent();
    }
}
