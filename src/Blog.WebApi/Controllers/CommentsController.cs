using Microsoft.AspNetCore.Mvc;
using Blog.Application;
using Blog.WebApi.Models.Input;
using Microsoft.AspNetCore.Authorization;

namespace Blog.WebApi.Controllers;

public class CommentsController(ICommentService commentsService, IPostService postService)
    : BaseController<CommentDto>(commentsService)
{
    private readonly ICommentService _commentService = commentsService;
    private readonly IPostService _postService = postService;

    /// <summary>
    /// Retrieves all comments for a specific post.
    /// </summary>
    /// <param name="postId">The ID of the post.</param>
    /// <returns>Returns a list of comments for the specified post.</returns>
    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(200)] // OK
    [ProducesResponseType(404)] // Not Found
    public async Task<IActionResult> GetAll([FromQuery] Guid postId)
    {
        var post = await _postService.Get(postId);
        if (post is null)
            return NotFound(new { Id = postId });

        return Ok(await _commentService.GetAll(postId));
    }

    /// <summary>
    /// Retrieves a specific comment by its ID.
    /// </summary>
    /// <param name="id">The ID of the comment to retrieve.</param>
    /// <returns>Returns the comment if found, otherwise returns a 404 Not Found.</returns>
    [AllowAnonymous]
    [HttpGet("{id}")]
    [ProducesResponseType(200)] // OK
    [ProducesResponseType(404)] // Not Found
    public new async Task<IActionResult> Get(Guid id)
        => await base.Get(id);

    /// <summary>
    /// Creates a new comment for a specific post.
    /// </summary>
    /// <param name="input">The input model containing data for the new comment.</param>
    /// <returns>Returns the newly created comment.</returns>
    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(201)] // Created
    [ProducesResponseType(400)] // Bad Request
    [ProducesResponseType(404)] // Not Found
    public async Task<IActionResult> Post([FromBody] CommentInputModel input)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var post = await _postService.Get(input.PostId);
        if (post is null)
            return NotFound(new { Id = input.PostId });

        return await base.Post(input);
    }

    /// <summary>
    /// Deletes a comment by its ID.
    /// </summary>
    /// <param name="id">The ID of the comment to delete.</param>
    /// <returns>Returns 204 No Content if successful, otherwise returns a 404 Not Found.</returns>
    [Authorize(Roles = Roles.Moderator)]
    [HttpDelete("{id}")]
    [ProducesResponseType(204)] // No Content
    [ProducesResponseType(404)] // Not Found
    public new async Task<IActionResult> Delete(Guid id)
        => await base.Delete(id);

}