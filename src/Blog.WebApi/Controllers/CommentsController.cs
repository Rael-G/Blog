using Microsoft.AspNetCore.Mvc;
using Blog.Application;
using Blog.WebApi.Models.Input;

namespace Blog.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController(ICommentService commentsService) 
        : ControllerBase
    {
        private readonly ICommentService _commentService = commentsService;

        [HttpGet]
        public async Task<IActionResult> Get()
            => Ok(await _commentService.GetAll());
        

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var comment = await _commentService.Get(id);

            if (comment is null)
                return NotFound();

            return Ok(comment);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CommentInputModel input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = input.InputToDto();
            try
            {
                _commentService.Create(comment);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            await _commentService.Commit();
            return CreatedAtAction(nameof(Get), new { comment.Id }, comment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] CommentInputModel input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existentComment = await _commentService.Get(id);
            if (existentComment is null)
                return NotFound();

            var comment = input.InputToDto(id);
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
                return NotFound();

            _commentService.Delete(comment);

            return NoContent();
        }
    }
}
