using AutoMapper;
using Blog.Domain;
using Microsoft.AspNetCore.Identity;

namespace Blog.Application;

public class UserService(IUserRepository _userRepository, IMapper mapper) 
    : BaseService<UserDto, User>(_userRepository, mapper), IUserService
{
    public const int PageSize = 10;
    private PasswordHasher<User> _passwordHasher { get; set;} = new PasswordHasher<User>();

    public async Task<UserDto> GetByUserName(string username)
        => Mapper.Map<UserDto>(await _userRepository.GetByUserName(username));
    
    public async Task<UserDto?> GetUserPage(Guid id, int page)
    {
        var user = await Repository.Get(id);

        if (user == null)
            return null;

        var posts = await _userRepository.GetPostPage(id, page, PageSize);
        var userDto = Mapper.Map<UserDto>(user);
        var postsDto = Mapper.Map<IEnumerable<PostDto>>(posts);
        userDto.Posts = postsDto;
        return userDto;
    }

    public async Task<int> GetPageCount(Guid id)
        => (int)Math.Ceiling(await _userRepository.GetPostCount(id) / (float)PageSize);

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
