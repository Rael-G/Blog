using Blog.Application;
using Blog.Domain;
using Blog.Persistance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.WebApi;

[Route("api/[controller]")]
[ApiController]
public class ArchivesController(IArchiveService ArchiveService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
        => Ok(await ArchiveService.GetAll());

    [AllowAnonymous]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(Guid id)
    {
        var archive = await ArchiveService.Get(id);

        if (archive is null)
            return NotFound(new {Id = id});
        
        if (!archive.IsPublic)
        {
            Guid claimId = Guid.Empty;
            try
            {
                claimId = TokenService.GetUserIdFromClaims(User);
            }
            catch(AppException)
            {
                return Forbid();
            }

            if (claimId != archive.OwnerId)
                return Forbid();
        }
        
        archive = await ArchiveService.GetWithFile(archive);
        if (archive is null || archive.Stream is null)
            return NotFound(new {Id = id, Message = "Stream is missing."});

        return File(archive.Stream, archive.ContentType, archive.FileName);
    }

    [Authorize(Roles = Roles.Moderator)]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post(IFormFile file, [FromForm] ArchiveInputModel input)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var archive = input.InputToDto();
        archive.OwnerId = TokenService.GetUserIdFromClaims(User);
        try
        {
            await ArchiveService.Create(archive, file);
        }
        catch (DomainException ex)
        {
            return BadRequest(new { ex.Message });
        }

        return CreatedAtAction(nameof(Get), new { Id = archive.Id }, new ArchiveOutputModel(archive));
    }

    [Authorize(Roles = Roles.Moderator)]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put(Guid id, IFormFile file, [FromForm] ArchiveInputModel input)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var existing = await ArchiveService.Get(id);
        if (existing is null)
            return NotFound(new { Id = id });

        var archive = input.InputToDto();
        if(TokenService.GetUserIdFromClaims(User) != existing.OwnerId)
            return Forbid();

        await ArchiveService.Update(archive, file);

        return NoContent();
    }

    [Authorize(Roles = Roles.Moderator)]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var archive = await ArchiveService.Get(id);

        if (archive is null)
            return NotFound(new {Id = id});

        var claimId = TokenService.GetUserIdFromClaims(User);
        if(claimId != archive.OwnerId && !User.IsInRole(Roles.Admin))
            return Forbid();

        await ArchiveService.Delete(archive);

        return NoContent();
    }
}
