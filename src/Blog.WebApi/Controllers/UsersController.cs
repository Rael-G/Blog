using Blog.Application;
using Blog.Domain;
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
    [Authorize]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserOutputModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public new async Task<IActionResult> Get(Guid id)
    {
        var claimId = TokenService.GetUserIdFromClaims(User);
        if(claimId != id && !User.IsInRole(Roles.Admin))
            return Forbid();

        var user = await Service.Get(id);

        if (user is null)
            return NotFound(new { Id = id });

        return Ok(new UserOutputModel(user));
    }

    /// <summary>
    /// Retrieves a page of posts associated with a specific user, identified by its ID, and returns them. 
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <param name="page">The page number.</param>
    /// 200 (OK) response containing the user with its associated posts on the specified page.
    /// 404 (Not Found) if the user with the specified ID does not exist.
    [AllowAnonymous]
    [HttpGet("{id}/page")]
    [ProducesResponseType(typeof(UserOutputModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPage(Guid id, [FromQuery] int page)
    {
        var user = await _userService.GetUserPage(id, page);

        if (user is null)
            return NotFound(new { Id = id });

        return Ok(new UserOutputModel(user));   
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
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPageCount(Guid id)
    {
        var user = await _userService.Get(id);

        if (user is null)
            return NotFound(new { Id = id });

        return Ok(await _userService.GetPageCount(id));
    }

    /// <summary>
    /// Retrieves a specific user by its username.
    /// </summary>
    /// <param name="username">The username of the user to retrieve.</param>
    /// <returns>Returns the user if found, otherwise returns a 404 Not Found.</returns>
    [Authorize]
    [HttpGet("username/{username}")]
    [ProducesResponseType(typeof(UserOutputModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByUserName(string username)
    {
        var user = await _userService.GetByUserName(username);

        if (user is null)
            return NotFound(new { UserName = username });

        var claimId = TokenService.GetUserIdFromClaims(User);
        if(claimId != user.Id && !User.IsInRole(Roles.Admin))
            return Forbid();

        return Ok(new UserOutputModel(user));
    }

    /// <summary>
    /// Retrieves all user.
    /// </summary>
    /// <returns>Returns a list of all user.</returns>
    [Authorize(Roles = Roles.Admin)]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserOutputModel>), StatusCodes.Status200OK)]
    public new async Task<IActionResult> GetAll()
        => Ok(UserOutputModel.MapRange(await Service.GetAll()));
    
    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="input">The input model containing data for the new user.</param>
    /// <returns>Returns the newly created user.</returns>
    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] SigninInputModel input)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = input.InputToDto();
        try
        {
            await Service.Create(user);
        }
        catch (DomainException ex)
        {
            return BadRequest(new { ex.Message });
        }

        user.PasswordHash = null;
        user.RepeatPassword = null;

        return CreatedAtAction(nameof(Get), new { user.Id }, new UserOutputModel(user));
    }

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="id">The ID of the user to update.</param>
    /// <param name="input">The input model containing updated data for the user.</param>
    /// <returns>Returns 204 No Content if successful, otherwise returns a 404 Not Found or 400 Bad Request.</returns>
    [Authorize]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put(Guid id, [FromBody] UserInputModel input)
    {
        var claimId = TokenService.GetUserIdFromClaims(User);
        if(claimId != id)
            return Forbid();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await Service.Get(id);
        if (user is null)
            return NotFound(new { Id = id });

        input.InputToDto(user);
        try
        {
            await Service.Update(user);
        }
        catch (DomainException ex)
        {
            return BadRequest(new { ex.Message });
        }

        return NoContent();
    }

    [Authorize]
    [HttpPut("reset-password/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ResetPassword(Guid id, [FromBody] ResetPasswordInputModel input)
    {
        var claimId = TokenService.GetUserIdFromClaims(User);
        if(claimId != id)
            return Forbid();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await Service.Get(id);
        if (user is null)
            return NotFound(new { Id = id });

        input.InputToDto(user);
        try
        {
            await _userService.UpdatePassword(user);
        }
        catch (DomainException ex)
        {
            return BadRequest(new { ex.Message });
        }

        return NoContent();
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPut("roles/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SetRoles(Guid id, [FromBody] string[] roles)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await Service.Get(id);
        if (user is null)
            return NotFound(new {Id = id});

        user.Roles = roles;
        try
        {
            await _userService.UpdateRoles(user);
        }
        catch (DomainException ex)
        {
            return BadRequest(new { ex.Message });
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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public new async Task<IActionResult> Delete(Guid id)
    {    
        var claimId = TokenService.GetUserIdFromClaims(User);
        if(claimId != id && !User.IsInRole(Roles.Admin))
            return Forbid();
            
        return await base.Delete(id);
    }
}
