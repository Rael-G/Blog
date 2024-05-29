using Blog.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.WebApi;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    /// <summary>
    /// Logs in a user with provided credentials.
    /// </summary>
    /// <param name="userInput">The user credentials for login.</param>
    /// <returns>Returns the logged-in user information, including a token.</returns>
    [AllowAnonymous]
    [HttpPut("login")]
    [ProducesResponseType(typeof(Token), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> LogIn(LoginInputModel userInput)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userDto = userInput.InputToDto();

        var token = await _authService.LoginAsync(userDto);

        if (token is null)
        {
            return Unauthorized(new {Message = "Wrong Username or Password."});
        }

        return Ok(token);
    }

    /// <summary>
    /// Controller endpoint for regenerating a new token based on the provided input model.
    /// </summary>
    /// <param name="tokenInput">The input model containing the access and refresh tokens.</param>
    /// <returns>Returns the regenerated token information.</returns>
    [AllowAnonymous]
    [HttpPut("regen-token")]
    [ProducesResponseType(typeof(Token), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> RegenerateToken(TokenInputModel tokenInput)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var token = await _authService.RegenarateTokenAsync(tokenInput.InputToDto());

        if (token is null)
        {
            return Unauthorized(new { Message = "Invalid Token"});
        }

        return Ok(token);
    }
}
