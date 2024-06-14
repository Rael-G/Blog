using System.Text.RegularExpressions;
using AutoMapper;
using Blog.Domain;
using Microsoft.AspNetCore.Identity;

namespace Blog.Application;

public partial class UserService(IUserRepository _userRepository, IMapper mapper) 
    : BaseService<UserDto, User>(_userRepository, mapper), IUserService
{
    public const int PageSize = 10;
    private PasswordHasher<User> _passwordHasher { get; set;} = new PasswordHasher<User>();

    public async Task<UserDto> GetByUserName(string username)
        => Mapper.Map<UserDto>(await _userRepository.GetByUserName(username));
    
    public async Task<UserDto?> GetUserPage(Guid id, int page)
    {
        var user = await Repository.Get(id);

        if (user == null) return null;

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
        if (await _userRepository.GetByUserName(userDto.UserName) is not null) 
            throw new DomainException("UserName must be unique");

        var user = Mapper.Map<User>(userDto);

        MapUserPassword(user, userDto);

        Repository.Create(user);
        await Repository.Commit();
    }

    public new async Task Update(UserDto userDto)
    {
        var existentUser = await _userRepository.GetByUserName(userDto.UserName);
        if (existentUser is not null && userDto.Id != existentUser.Id) 
            throw new DomainException("UserName must be unique");

        var user = await Repository.Get(userDto.Id) ?? throw new ArgumentException("User not Found");
        MapUser(user, userDto);
        Repository.Update(user);
        await Repository.Commit();
    }

    public async Task UpdatePassword(UserDto userDto)
    {
        var user = await Repository.Get(userDto.Id) ?? throw new ArgumentException("User not Found");
        MapUserPassword(user, userDto);
        Repository.Update(user);
        await Repository.Commit();
    }

    public async Task UpdateRoles(UserDto userDto)
    {
        var user = await Repository.Get(userDto.Id) ?? throw new ArgumentException("User not Found");
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
        if (!PasswordIsValid(userDto.PasswordHash))
            throw new DomainException
            (
                "Password must contain 1 number (0-9), 1 uppercase letters, 1 lowercase letters, " +
                "1 non-alpha numeric number and must has more than 8 characters with no space"
            );

        if (!PasswordAreEqual(userDto.PasswordHash, userDto.RepeatPassword))
            throw new DomainException("Password and Repeat Password must be equal");
        

        var passwordHash = _passwordHasher
            .HashPassword(user, userDto.PasswordHash!);

        user.PasswordHash = passwordHash;
    }

    private static void MapUserRoles(User user, UserDto userDto)
    {
        if (userDto.Roles != null)
            user.Roles = userDto.Roles;
    }

    private static bool PasswordAreEqual(string? password, string? repeatPassword)
    {
        if (password is null) return false;

        return password == repeatPassword;
    }

    private static bool PasswordIsValid(string? password)
    {
        if (password is null) return false;

        return User.PasswordRegex().Match(password).Success;
    }
}
