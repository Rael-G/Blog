using AutoMapper;
using Blog.Domain;
using Microsoft.AspNetCore.Identity;

namespace Blog.Application;

public class UserService(IUserRepository _repository, IMapper mapper) 
    : BaseService<UserDto, User>(_repository, mapper), IUserService
{
    private PasswordHasher<User> _passwordHasher { get; set;} = new PasswordHasher<User>();

    public async Task<UserDto> GetByUserName(string username)
        => Mapper.Map<UserDto>(await _repository.GetByUserName(username));
    

    public new async Task Create(UserDto userDto)
    {
        var user = Mapper.Map<User>(userDto);

        MapUserPassword(user, userDto);

        Repository.Create(user);
        await Repository.Commit();
    }

    public new async Task Update(UserDto userDto)
    {
        var user = await Repository.Get(userDto.Id);
        if (user == null)
            throw new ArgumentException("User not Found");

        MapUser(user, userDto);
        Repository.Update(user);
        await Repository.Commit();
    }

    public async Task UpdatePassword(UserDto userDto)
    {
        var user = await Repository.Get(userDto.Id);
        if (user == null)
            throw new ArgumentException("User not Found");

        MapUserPassword(user, userDto);
        Repository.Update(user);
        await Repository.Commit();
    }

    public async Task UpdateRoles(UserDto userDto)
    {
        var user = await Repository.Get(userDto.Id);
        if (user == null)
            throw new ArgumentException("User not Found");

        MapUserRoles(user, userDto);
        Repository.Update(user);
        await Repository.Commit();
    }

    private void MapUser(User user, UserDto userDto)
    {
        user.UserName = userDto.UserName;
    }

    private void MapUserPassword(User user, UserDto userDto)
    {
        if (userDto.PasswordHash == null 
            || userDto.PasswordHash != userDto.RepeatPassword)
        {
            throw new ArgumentException("Password and Repeat Password must be equal");
        }

        var passwordHash = _passwordHasher
            .HashPassword(user, userDto.PasswordHash!);

        user.PasswordHash = passwordHash;
    }

    private void MapUserRoles(User user, UserDto userDto)
    {
        if (userDto.Roles != null)
            user.Roles = userDto.Roles;
    }
}
