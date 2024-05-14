using Blog.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.WebApi;

public class UsersController(IUserService _userService) 
    : BaseController<UserDto>(_userService)
{
    /// <summary>
    /// Retrieves a specific user by its ID.
    /// </summary>
    /// <param name="id">The ID of the user to retrieve.</param>
    /// <returns>Returns the user if found, otherwise returns a 404 Not Found.</returns>
    [Authorize(Roles = Roles.Moderator)]
    [HttpGet("{id}")]
    [ProducesResponseType(200)] // OK
    [ProducesResponseType(404)] // Not Found
    public new async Task<IActionResult> Get(Guid id)
        => await base.Get(id);

    /// <summary>
    /// Retrieves a page of posts associated with a specific user, identified by its ID, and returns them. 
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <param name="page">The page number.</param>
    /// 200 (OK) response containing the user with its associated posts on the specified page.
    /// 404 (Not Found) if the user with the specified ID does not exist.
    [AllowAnonymous]
    [HttpGet("{id}/page")]
    [ProducesResponseType(200)] // OK
    [ProducesResponseType(404)] // Not Found
    public async Task<IActionResult> GetPage(Guid id, [FromQuery] int page)
    {
        var user = await _userService.GetUserPage(id, page);

        if (user is null)
            return NotFound(id);

        return Ok(user);   
    }

    /// <summary>
    /// Retrieves the total number of pages for posts associated with a specific user, identified by its ID.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <returns>
    /// 200 (OK) response containing the total number of pages for posts associated with the user.
    /// 404 (Not Found) if the user with the specified ID does not exist.
    /// </returns>
    [AllowAnonymous]
    [HttpGet("{id}/page-count")]
    [ProducesResponseType(200)] // OK
    [ProducesResponseType(404)] // Not Found
    public async Task<IActionResult> GetPageCount(Guid id)
    {
        var user = await _userService.Get(id);

        if (user is null)
            return NotFound(id);

        return Ok(await _userService.GetPageCount(id));
    }

    /// <summary>
    /// Retrieves a specific user by its username.
    /// </summary>
    /// <param name="username">The username of the user to retrieve.</param>
    /// <returns>Returns the user if found, otherwise returns a 404 Not Found.</returns>
    [Authorize(Roles = Roles.Moderator)]
    [HttpGet("username/{username}")]
    [ProducesResponseType(200)] // OK
    [ProducesResponseType(404)] // Not Found
    public async Task<IActionResult> GetByUserName(string username)
    {
        var entity = await _userService.GetByUserName(username);

        if (entity is null)
            return NotFound(username);

        return Ok(entity);
    }

    /// <summary>
    /// Retrieves all user.
    /// </summary>
    /// <returns>Returns a list of all user.</returns>
    [Authorize(Roles = Roles.Admin)]
    [HttpGet]
    [ProducesResponseType(200)] // OK
    public new async Task<IActionResult> GetAll()
        => await base.GetAll();
        
    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="input">The input model containing data for the new user.</param>
    /// <returns>Returns the newly created user.</returns>
    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(201)] // Created
    [ProducesResponseType(400)] // Bad Request
    public async Task<IActionResult> Post([FromBody] UserInputModel input)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var entity = input.InputToDto();
        try
        {
            await Service.Create(entity);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }

        entity.PasswordHash = null;
        entity.RepeatPassword = null;

        return CreatedAtAction(nameof(Get), new { entity.Id }, entity);
    }

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="id">The ID of the user to update.</param>
    /// <param name="input">The input model containing updated data for the user.</param>
    /// <returns>Returns 204 No Content if successful, otherwise returns a 404 Not Found or 400 Bad Request.</returns>
    [Authorize]
    [HttpPut("{id}")]
    [ProducesResponseType(204)] // No Content
    [ProducesResponseType(400)] // Bad Request
    [ProducesResponseType(404)] // Not Found
    public async Task<IActionResult> Put(Guid id, [FromBody] UserInputModel input)
        => await base.Put(id, input);

    [Authorize]
    [HttpPut("reset-password/{id}")]
    [ProducesResponseType(204)] // No Content
    [ProducesResponseType(400)] // Bad Request
    [ProducesResponseType(404)] // Not Found
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ResetPassword(Guid id, [FromBody] UserInputModel input)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var entity = await Service.Get(id);
        if (entity is null)
            return NotFound(id);

        if (TokenService.GetUserIdFromClaims(User) != entity.Id)
            return Unauthorized();

        input.InputToDto(entity);
        try
        {
            await _userService.UpdatePassword(entity);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }

        return NoContent();
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPut("roles/{id}")]
    [ProducesResponseType(204)] // No Content
    [ProducesResponseType(400)] // Bad Request
    [ProducesResponseType(404)] // Not Found
    public async Task<IActionResult> SetRoles(Guid id, [FromBody] string[] roles)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var entity = await Service.Get(id);
        if (entity is null)
            return NotFound(id);

        entity.Roles = roles;
        try
        {
            await _userService.UpdateRoles(entity);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }

        return NoContent();
    }

    /// <summary>
    /// Deletes a user by its ID.
    /// </summary>
    /// <param name="id">The ID of the user to delete.</param>
    /// <returns>Returns 204 No Content if successful, otherwise returns a 404 Not Found.</returns>
    [Authorize]
    [HttpDelete("{id}")]
    [ProducesResponseType(204)] // No Content
    [ProducesResponseType(404)] // Not Found
    public new async Task<IActionResult> Delete(Guid id)
        => await base.Delete(id);
}
