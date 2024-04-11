using Blog.Application;
using Microsoft.AspNetCore.Mvc;

namespace Blog.WebApi;

[ApiController]
public class TagsController(ITagService tagService) : BaseController<TagDto>(tagService)
{
    /// <summary>
    /// Retrieves a specific tag by its ID.
    /// </summary>
    /// <param name="id">The ID of the post to retrieve.</param>
    /// <returns>Returns the post if found, otherwise returns a 404 Not Found.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(200)] // OK
    [ProducesResponseType(404)] // Not Found
    public new async Task<IActionResult> Get(Guid id)
        => await base.Get(id);

    /// <summary>
    /// Retrieves all tags.
    /// </summary>
    /// <returns>Returns a list of all posts.</returns>
    [HttpGet]
    [ProducesResponseType(200)] // OK
    public new async Task<IActionResult> GetAll()
        => await base.GetAll();

    /// <summary>
    /// Retrieves a page of posts.
    /// </summary>
    /// <returns>Returns a list of posts within a page.</returns>
    [HttpGet("{id}/page")]
    [ProducesResponseType(200)] // OK
    public async Task<IActionResult> GetPage(Guid id, [FromQuery] int page)
    {
        var tag = await tagService.GetTagPage(id, page);

        if (tag is null)
            return NotFound(id);

        return Ok(tag);   
    }

    /// <summary>
    /// Retrieves a page count.
    /// </summary>
    /// <returns>Returns the count of pages.</returns>
    [HttpGet("{id}/page-count")]
    [ProducesResponseType(200)] // OK
    public async Task<IActionResult> GetPageCount(Guid id)
        => Ok(await tagService.GetPageCount(id));

    /// <summary>
    /// Creates a new tag.
    /// </summary>
    /// <param name="input">The input model containing data for the new tag.</param>
    /// <returns>Returns the newly created tag.</returns>
    [HttpPost]
    [ProducesResponseType(201)] // Created
    [ProducesResponseType(400)] // Bad Request
    public async Task<IActionResult> Post([FromBody] TagInputModel input)
        => await base.Post(input);

    /// <summary>
    /// Updates an existing tag.
    /// </summary>
    /// <param name="id">The ID of the tag to update.</param>
    /// <param name="input">The input model containing updated data for the tag.</param>
    /// <returns>Returns 204 No Content if successful, otherwise returns a 404 Not Found or 400 Bad Request.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(204)] // No Content
    [ProducesResponseType(400)] // Bad Request
    [ProducesResponseType(404)] // Not Found
    public async Task<IActionResult> Put(Guid id, [FromBody] TagInputModel input)
        => await base.Put(id, input);

    /// <summary>
    /// Deletes a tag by its ID.
    /// </summary>
    /// <param name="id">The ID of the tag to delete.</param>
    /// <returns>Returns 204 No Content if successful, otherwise returns a 404 Not Found.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(204)] // No Content
    [ProducesResponseType(404)] // Not Found
    public new async Task<IActionResult> Delete(Guid id)
        => await base.Delete(id);
}