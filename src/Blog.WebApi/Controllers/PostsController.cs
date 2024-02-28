using Microsoft.AspNetCore.Mvc;
using Blog.Application;
using Blog.WebApi.Models.Input;

namespace Blog.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController(IPostService postService) 
        : ControllerBase
    {
        private readonly IPostService _postService = postService;

        [HttpGet]
        public async Task<IActionResult> Get()
            => Ok(await _postService.GetAll());
        

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var post = await _postService.Get(id);

            if (post is null)
                return NotFound();

            return Ok(post);
        }

        [HttpPost]
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] PostInputModel input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existentPost = await _postService.Get(id);
            if (existentPost is null)
                return NotFound();

            var post = input.InputToDto(id);
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var post = await _postService.Get(id);

            if (post is null)
                return NotFound();

            _postService.Delete(post);

            return NoContent();
        }
    }
}
