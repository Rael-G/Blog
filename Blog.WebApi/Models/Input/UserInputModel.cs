using System.ComponentModel.DataAnnotations;
using Blog.Application;

namespace Blog.WebApi;

public class UserInputModel
{
    [Required]
    public string UserName { get; set;} = string.Empty;

    public UserDto InputToDto()
        => new() { Id = Guid.NewGuid(), UserName = UserName};

    public void InputToDto(UserDto userDto)
    {
        userDto.UserName = UserName;
    }
}
