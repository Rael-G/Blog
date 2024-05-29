using Microsoft.AspNetCore.Mvc;
using Blog.Application;
using Blog.WebApi.Models.Input;
using Microsoft.AspNetCore.Authorization;

namespace Blog.WebApi.Controllers;

[ApiController]
public class PostsController(IPostService postService)
    : BaseController<PostDto>(postService)
{
    private readonly IPostService _postService = postService;
    /// <summary>
    /// Retrieves a specific post by its ID.
    /// </summary>
    /// <param name="id">The ID of the post to retrieve.</param>
    /// <returns>Returns the post if found, otherwise returns a 404 Not Found.</returns>
    [AllowAnonymous]
    [HttpGet("{id}")]
    [ProducesResponseType(200)] // OK
    [ProducesResponseType(404)] // Not Found
    public new async Task<IActionResult> Get(Guid id)
        => await base.Get(id);

    /// <summary>
    /// Retrieves a page of posts, according to the specified page number.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <returns>
    /// 200 (OK) response containing the posts on the specified page.
    /// </returns>
    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(200)] // OK
    public async Task<IActionResult> GetPage([FromQuery] int page)
        => Ok(await _postService.GetPage(page));

    /// <summary>
    /// Retrieves the total number of pages for all posts.
    /// </summary>
    /// <returns>
    /// 200 (OK) response containing the total number of pages for all posts.
    /// </returns>
    [AllowAnonymous]
    [HttpGet("page-count")]
    [ProducesResponseType(200)] // OK
    public async Task<IActionResult> GetPageCount()
        => Ok(await _postService.GetPageCount());

    /// <summary>
    /// Creates a new blog post.
    /// </summary>
    /// <param name="input">The input model containing data for the new blog post.</param>
    /// <returns>Returns the newly created blog post.</returns>
    [Authorize(Roles = Roles.Moderator)]
    [HttpPost]
    [ProducesResponseType(201)] // Created
    [ProducesResponseType(400)] // Bad Request
    public async Task<IActionResult> Post([FromBody] PostInputModel input)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var entity = input.InputToDto();
        entity.UserId = TokenService.GetUserIdFromClaims(User);
        try
        {
            
            await Service.Create(entity);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex);
        }

        return CreatedAtAction(nameof(Get), new { entity.Id }, entity);
    }

    /// <summary>
    /// Updates an existing blog post.
    /// </summary>
    /// <param name="id">The ID of the blog post to update.</param>
    /// <param name="input">The input model containing updated data for the blog post.</param>
    /// <returns>Returns 204 No Content if successful, otherwise returns a 404 Not Found or 400 Bad Request.</returns>
    [Authorize]
    [HttpPut("{id}")]
    [ProducesResponseType(204)] // No Content
    [ProducesResponseType(400)] // Bad Request
    [ProducesResponseType(404)] // Not Found
    public async Task<IActionResult> Put(Guid id, [FromBody] PostInputModel input)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var entity = await Service.Get(id);
        if (entity is null)
            return NotFound(new { Id = id });

        if(TokenService.GetUserIdFromClaims(User) != entity.UserId)
            return Forbid();

        input.InputToDto(entity);
        try
        {
            await Service.Update(entity);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex);
        }

        return NoContent();   
    }

    /// <summary>
    /// Deletes a post by its ID.
    /// </summary>
    /// <param name="id">The ID of the post to delete.</param>
    /// <returns>Returns 204 No Content if successful, otherwise returns a 404 Not Found.</returns>
    [Authorize]
    [HttpDelete("{id}")]
    [ProducesResponseType(204)] // No Content
    [ProducesResponseType(404)] // Not Found
    public new async Task<IActionResult> Delete(Guid id)
    {
        var post = await Service.Get(id);

        if (post is null)
            return NotFound(new { Id = id });
        
        var claimId = TokenService.GetUserIdFromClaims(User);
        if(claimId != post.UserId && !User.IsInRole(Roles.Admin))
            return Forbid();

        await Service.Delete(post);

        return NoContent();
    }
}