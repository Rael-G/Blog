using Microsoft.AspNetCore.Mvc;
using Blog.Application;
using Blog.WebApi.Models.Input;

namespace Blog.WebApi.Controllers;

[ApiController]
public class PostsController(IPostService postService)
    : BaseController<PostDto>(postService)
{
    /// <summary>
    /// Retrieves a specific post by its ID.
    /// </summary>
    /// <param name="id">The ID of the post to retrieve.</param>
    /// <returns>Returns the post if found, otherwise returns a 404 Not Found.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(200)] // OK
    [ProducesResponseType(404)] // Not Found
    public new async Task<IActionResult> Get(Guid id)
        => await base.Get(id);

    /// <summary>
    /// Retrieves all posts.
    /// </summary>
    /// <returns>Returns a list of all posts.</returns>
    [HttpGet]
    [ProducesResponseType(200)] // OK
    public new async Task<IActionResult> GetAll()
        => await base.GetAll();

    /// <summary>
    /// Creates a new blog post.
    /// </summary>
    /// <param name="input">The input model containing data for the new blog post.</param>
    /// <returns>Returns the newly created blog post.</returns>
    [HttpPost]
    [ProducesResponseType(201)] // Created
    [ProducesResponseType(400)] // Bad Request
    public async Task<IActionResult> Post([FromBody] PostInputModel input)
        => await base.Post(input);

    /// <summary>
    /// Updates an existing blog post.
    /// </summary>
    /// <param name="id">The ID of the blog post to update.</param>
    /// <param name="input">The input model containing updated data for the blog post.</param>
    /// <returns>Returns 204 No Content if successful, otherwise returns a 404 Not Found or 400 Bad Request.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(204)] // No Content
    [ProducesResponseType(400)] // Bad Request
    [ProducesResponseType(404)] // Not Found
    public async Task<IActionResult> Put(Guid id, [FromBody] PostInputModel input)
        => await base.Put(id, input);

    /// <summary>
    /// Deletes a post by its ID.
    /// </summary>
    /// <param name="id">The ID of the post to delete.</param>
    /// <returns>Returns 204 No Content if successful, otherwise returns a 404 Not Found.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(204)] // No Content
    [ProducesResponseType(404)] // Not Found
    public new async Task<IActionResult> Delete(Guid id)
        => await base.Delete(id);
}