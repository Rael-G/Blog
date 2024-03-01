using Microsoft.AspNetCore.Mvc;
using Blog.Application;
using Blog.WebApi.Models.Input;
using AutoMapper;
using Blog.Domain;

namespace Blog.WebApi.Controllers
{
    [Route("api/posts/{postId}/comments")]
    [ApiController]
    public class CommentsController(ICommentService commentsService, IPostService postService, IMapper mapper) 
        : ControllerBase
    {
        private readonly ICommentService _commentService = commentsService;
        private readonly IPostService _postService = postService;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<IActionResult> GetAll([FromRoute] Guid postId)
        {
            var post = await _postService.Get(postId);
            if (post is null)
                return NotFound(postId);
            return Ok(await _commentService.GetAll(postId));
        }
        

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var comment = await _commentService.Get(id);

            if (comment is null)
                return NotFound(id);

            return Ok(comment);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromRoute]Guid postId, [FromBody] CommentInputModel input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var post = await _postService.Get(postId);
            if (post is null)
                return NotFound(postId);

            var comment = input.InputToDto();
            comment.PostId = postId;
            try
            {
                _commentService.Create(comment);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            await _commentService.Commit();
            return CreatedAtAction(nameof(Get), new { postId, comment.Id }, comment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] CommentInputModel input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = await _commentService.Get(id);
            if (comment is null)
                return NotFound(id);

            input.InputToDto(comment);
            try
            {
                _commentService.Update(comment);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            await _commentService.Commit();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var comment = await _commentService.Get(id);

            if (comment is null)
                return NotFound(id);

            _commentService.Delete(comment);

            await _commentService.Commit();
            return NoContent();
        }
    }
}
