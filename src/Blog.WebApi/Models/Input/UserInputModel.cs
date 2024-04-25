using Blog.Application;

namespace Blog.WebApi;

public class UserInputModel
{
    public string UserName { get; set;} = string.Empty;
    public string Password { get; set;} = string.Empty;
    public string RepeatPassword { get; set;} = string.Empty;

    public UserDto InputToDto()
        => new() { Id = Guid.NewGuid(), UserName = UserName, PasswordHash = Password, RepeatPassword = RepeatPassword };

    public void InputToDto(UserDto userDto)
    {
        userDto.UserName = UserName;
        userDto.PasswordHash = Password;
        userDto.RepeatPassword = RepeatPassword;
    }
}
