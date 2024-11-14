using System.ComponentModel.DataAnnotations;
using Blog.Application;

namespace Blog.WebApi;

public class ResetPasswordInputModel
{
    [Required]
    public string Password { get; set;} = string.Empty;

    [Required]
    public string RepeatPassword { get; set;} = string.Empty;

    public UserDto InputToDto()
        => new() { Id = Guid.NewGuid(), PasswordHash = Password, RepeatPassword = RepeatPassword };

    public void InputToDto(UserDto userDto)
    {
        userDto.PasswordHash = Password;
        userDto.RepeatPassword = RepeatPassword;
    }
}
