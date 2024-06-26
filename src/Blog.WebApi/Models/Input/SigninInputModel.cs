﻿using System.ComponentModel.DataAnnotations;
using Blog.Application;

namespace Blog.WebApi;

public class SigninInputModel : IInputModel<UserDto>
{
    [Required]
    public string UserName { get; set;} = string.Empty;

    [Required]
    public string Password { get; set;} = string.Empty;

    [Required]
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
