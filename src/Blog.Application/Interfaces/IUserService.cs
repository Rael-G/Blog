using Blog.Domain;

namespace Blog.Application;

public interface IUserService : IBaseService<UserDto>
{
    public Task UpdatePassword(UserDto userDto);

    public Task UpdateRoles(UserDto userDto);
}
