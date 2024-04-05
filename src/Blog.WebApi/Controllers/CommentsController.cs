using Microsoft.AspNetCore.Mvc;
using Blog.Application;
using Blog.WebApi.Models.Input;

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
    [HttpGet]
    [ProducesResponseType(200)] // OK
    [ProducesResponseType(404)] // Not Found
    public async Task<IActionResult> GetAll([FromQuery] Guid postId)
    {
        var post = await _postService.Get(postId);
        if (post is null)
            return NotFound(postId);

        return Ok(await _commentService.GetAll(postId));
    }

    /// <summary>
    /// Retrieves a specific comment by its ID.
    /// </summary>
    /// <param name="id">The ID of the comment to retrieve.</param>
    /// <returns>Returns the comment if found, otherwise returns a 404 Not Found.</returns>
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
            return NotFound(input.PostId);

        return await base.Post(input);
    }

    /// <summary>
    /// Updates an existing comment.
    /// </summary>
    /// <param name="id">The ID of the comment to update.</param>
    /// <param name="input">The input model containing updated data for the comment.</param>
    /// <returns>Returns 204 No Content if successful, otherwise returns a 400 Bad Request or 404 Not Found.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(204)] // No Content
    [ProducesResponseType(400)] // Bad Request
    [ProducesResponseType(404)] // Not Found
    public async Task<IActionResult> Put(Guid id, [FromBody] CommentInputModel input)
        => await base.Put(id, input);

    /// <summary>
    /// Deletes a comment by its ID.
    /// </summary>
    /// <param name="id">The ID of the comment to delete.</param>
    /// <returns>Returns 204 No Content if successful, otherwise returns a 404 Not Found.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(204)] // No Content
    [ProducesResponseType(404)] // Not Found
    public new async Task<IActionResult> Delete(Guid id)
        => await base.Delete(id);

}