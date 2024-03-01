using Microsoft.AspNetCore.Mvc;
using Blog.Application;
using Blog.WebApi.Models.Input;

namespace Blog.WebApi.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostsController(IPostService postService) 
        : ControllerBase
    {
        private readonly IPostService _postService = postService;

        /// <summary>
        /// Retrieves all blog posts.
        /// </summary>
        /// <returns>Returns a list of all blog posts.</returns>
        [HttpGet]
        [ProducesResponseType(200)] // OK
        public async Task<IActionResult> Get()
           => Ok(await _postService.GetAll());

        // <summary>
        /// Retrieves a specific blog post by its ID.
        /// </summary>
        /// <param name="id">The ID of the blog post to retrieve.</param>
        /// <returns>Returns the blog post if found, otherwise returns a 404 Not Found.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200)] // OK
        [ProducesResponseType(404)] // Not Found
        public async Task<IActionResult> Get(Guid id)
        {
            var post = await _postService.Get(id);

            if (post is null)
                return NotFound(id);

            return Ok(post);
        }

        /// <summary>
        /// Creates a new blog post.
        /// </summary>
        /// <param name="input">The input model containing data for the new blog post.</param>
        /// <returns>Returns the newly created blog post.</returns>
        [HttpPost]
        [ProducesResponseType(201)] // Created
        [ProducesResponseType(400)] // Bad Request
        public async Task<IActionResult> Post([FromBody] PostInputModel input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var post = input.InputToDto();
            try
            {
                _postService.Create(post);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            await _postService.Commit();
            return CreatedAtAction(nameof(Get), new { post.Id }, post);
        }

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
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var post = await _postService.Get(id);
            if (post is null)
                return NotFound(id);

            input.InputToDto(post);
            try
            {
                _postService.Update(post);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            await _postService.Commit();
            return NoContent();
        }

        /// <summary>
        /// Deletes a blog post by its ID.
        /// </summary>
        /// <param name="id">The ID of the blog post to delete.</param>
        /// <returns>Returns 204 No Content if successful, otherwise returns a 404 Not Found.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)] // No Content
        [ProducesResponseType(404)] // Not Found
        public async Task<IActionResult> Delete(Guid id)
        {
            var post = await _postService.Get(id);

            if (post is null)
                return NotFound(id);

            _postService.Delete(post);

            await _postService.Commit();
            return NoContent();
        }
    }
}
