using AutoMapper;
using Blog.Application;
using Blog.Domain;
using FluentAssertions;
using Moq;

namespace Blog.UnitTest.Application.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly UserService _userService;

    private readonly User _user;
    private readonly UserDto _userDto;
    private readonly List<Post> _posts;
    private readonly List<PostDto> _postsDto;

    public UserServiceTests()
    {
        _mockRepository = new Mock<IUserRepository>();
        _mockMapper = new Mock<IMapper>();
        _userService = new UserService(_mockRepository.Object, _mockMapper.Object);

        _user = new(Guid.NewGuid(), "username", Guid.NewGuid().ToString(), ["admin", "moderator"]);
        _userDto = new UserDto { Id = _user.Id, UserName = _user.UserName, Roles = _user.Roles};

        _posts = new List<Post>
            {
                new(Guid.NewGuid(), "Post 1", "Content 1", _user.Id),
                new(Guid.NewGuid(), "Post 2", "Content 2", _user.Id)
            };
            _postsDto = new List<PostDto>
            {
                new() { Id = _posts[0].Id, Title = "Post 1", Content = "Content 1", UserId = _user.Id },
                new() { Id = _posts[1].Id, Title = "Post 2", Content = "Content 2", UserId = _user.Id }
            };
    }

    [Fact]
    public async Task GetByUserName_WhenUserExists_ReturnsUserDto()
    {
        var username = "username";
        _mockRepository.Setup(repo => repo.GetByUserName(username)).ReturnsAsync(_user);
        _mockMapper.Setup(mapper => mapper.Map<UserDto>(_user)).Returns(_userDto);

        var result = await _userService.GetByUserName(username);

        result.Should().BeEquivalentTo(_userDto);
        _mockRepository.Verify(repo => repo.GetByUserName(username), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<UserDto>(_user), Times.Once);
    }

    [Fact]
    public async Task GetByUserName_WhenUserDoesNotExist_ReturnsNull()
    {
        var username = "nonexistent";
        _mockRepository.Setup(repo => repo.GetByUserName(username)).ReturnsAsync(()=> null);

        var result = await _userService.GetByUserName(username);

        result.Should().BeNull();
        _mockRepository.Verify(repo => repo.GetByUserName(username), Times.Once);
    }

    [Fact]
    public async Task GetUserPage_WhenUserExistsAndPostsExist_ReturnsUserDtoWithPosts()
    {
        var userId = _user.Id;
        var page = 1;
        const int pageSize = 10;

        _mockRepository.Setup(repo => repo.Get(userId)).ReturnsAsync(_user);
        _mockRepository.Setup(repo => repo.GetPostPage(userId, page, pageSize)).ReturnsAsync(_posts);
        _mockMapper.Setup(mapper => mapper.Map<UserDto>(_user)).Returns(_userDto);
        _mockMapper.Setup(mapper => mapper.Map<IEnumerable<PostDto>>(_posts)).Returns(_postsDto);

        var result = await _userService.GetUserPage(userId, page);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(_userDto);
        _mockRepository.Verify(repo => repo.Get(userId), Times.Once);
        _mockRepository.Verify(repo => repo.GetPostPage(userId, page, pageSize), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<UserDto>(_user), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<IEnumerable<PostDto>>(_posts), Times.Once);
    }

    [Fact]
    public async Task GetUserPage_WhenUserDoesNotExist_ReturnsNull()
    {
        var userId = Guid.NewGuid();
        var page = 1;

        _mockRepository.Setup(repo => repo.Get(userId)).ReturnsAsync(() => null);

        var result = await _userService.GetUserPage(userId, page);

        result.Should().BeNull();
        _mockRepository.Verify(repo => repo.Get(userId), Times.Once);
        _mockRepository.Verify(repo => repo.GetPostPage(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        _mockMapper.Verify(mapper => mapper.Map<UserDto>(It.IsAny<User>()), Times.Never);
        _mockMapper.Verify(mapper => mapper.Map<IEnumerable<PostDto>>(It.IsAny<IEnumerable<Post>>()), Times.Never);
    }

    [Fact]
    public async Task GetPageCount_WhenUserHasPosts_ReturnsCorrectPageCount()
    {
        var userId = Guid.NewGuid();
        const int postCount = 23;
        const int expectedPageCount = 3;

        _mockRepository.Setup(repo => repo.GetPostCount(userId)).ReturnsAsync(postCount);

        var result = await _userService.GetPageCount(userId);

        result.Should().Be(expectedPageCount);
        _mockRepository.Verify(repo => repo.GetPostCount(userId), Times.Once);
    }

    [Fact]
    public async Task GetPageCount_WhenUserHasNoPosts_ReturnsZero()
    {
        var userId = Guid.NewGuid();
        const int postCount = 0;
        const int expectedPageCount = 0;

        _mockRepository.Setup(repo => repo.GetPostCount(userId)).ReturnsAsync(postCount);

        var result = await _userService.GetPageCount(userId);

        result.Should().Be(expectedPageCount);
        _mockRepository.Verify(repo => repo.GetPostCount(userId), Times.Once);
    }

    [Fact]
    public async Task GetPageCount_WhenUserHasPostsExactlyOnePage_ReturnsOne()
    {
        var userId = Guid.NewGuid();
        const int pageSize = 10;
        const int postCount = pageSize;
        const int expectedPageCount = 1;

        _mockRepository.Setup(repo => repo.GetPostCount(userId)).ReturnsAsync(postCount);

        var result = await _userService.GetPageCount(userId);

        result.Should().Be(expectedPageCount);
        _mockRepository.Verify(repo => repo.GetPostCount(userId), Times.Once);
    }

    [Fact]
    public async Task Create_WhenUserNameIsNotUnique_ThrowsDomainException()
    {
        var userDto = new UserDto { UserName = "existingUser" };
        _mockRepository.Setup(repo => repo.GetByUserName(userDto.UserName)).ReturnsAsync(_user);

        Func<Task> act = async () => await _userService.Create(userDto);

        await act.Should().ThrowAsync<DomainException>().WithMessage("UserName must be unique");
        _mockRepository.Verify(repo => repo.GetByUserName(userDto.UserName), Times.Once);
        _mockRepository.Verify(repo => repo.Create(It.IsAny<User>()), Times.Never);
        _mockRepository.Verify(repo => repo.Commit(), Times.Never);
    }

    [Fact]
    public async Task Create_WhenPasswordIsInvalid_ThrowsDomainException()
    {
        var userDto = new UserDto { UserName = "newUser", PasswordHash = "invalid" };

        Func<Task> act = async () => await _userService.Create(userDto);

        await act.Should().ThrowAsync<DomainException>().WithMessage("Password must contain 1 number (0-9), 1 uppercase letters, 1 lowercase letters, 1 non-alpha numeric number and must has more than 8 characters with no space");
        _mockRepository.Verify(repo => repo.Create(It.IsAny<User>()), Times.Never);
        _mockRepository.Verify(repo => repo.Commit(), Times.Never);
    }

    [Fact]
    public async Task Create_WhenPasswordsDoNotMatch_ThrowsDomainException()
    {
        var userDto = new UserDto { UserName = "newUser", PasswordHash = "Valid1Password!", RepeatPassword = "DifferentPassword!" };

        Func<Task> act = async () => await _userService.Create(userDto);

        await act.Should().ThrowAsync<DomainException>().WithMessage("Password and Repeat Password must be equal");
        _mockRepository.Verify(repo => repo.Create(It.IsAny<User>()), Times.Never);
        _mockRepository.Verify(repo => repo.Commit(), Times.Never);
    }

    [Fact]
    public async Task Create_WhenValidUserDto_CreatesUser()
    {
        var userDto = new UserDto { UserName = "newUser", PasswordHash = "Valid1Password!", RepeatPassword = "Valid1Password!" };
        var user = new User(Guid.NewGuid(), userDto.UserName, userDto.PasswordHash);

        _mockMapper.Setup(mapper => mapper.Map<User>(userDto)).Returns(user);

        await _userService.Create(userDto);

        _mockRepository.Verify(repo => repo.GetByUserName(userDto.UserName), Times.Once);
        _mockRepository.Verify(repo => repo.Create(user), Times.Once);
        _mockRepository.Verify(repo => repo.Commit(), Times.Once);
    }

    [Fact]
    public async Task Update_WhenUserNameIsNotUnique_ThrowsDomainException()
    {
        var existingUser = new User(Guid.NewGuid(), "existingUsername", "hashedPassword");
        var userDto = new UserDto { Id = Guid.NewGuid(), UserName = "existingUsername" };

        _mockRepository.Setup(repo => repo.GetByUserName(userDto.UserName)).ReturnsAsync(existingUser);

        Func<Task> act = async () => await _userService.Update(userDto);

        await act.Should().ThrowAsync<DomainException>().WithMessage("UserName must be unique");
        _mockRepository.Verify(repo => repo.GetByUserName(userDto.UserName), Times.Once);
        _mockRepository.Verify(repo => repo.Update(It.IsAny<User>()), Times.Never);
        _mockRepository.Verify(repo => repo.Commit(), Times.Never);
    }

    [Fact]
    public async Task Update_WhenSameUserNameAndSameId_DoesNotThrowException()
    {
        var userId = Guid.NewGuid();
        var existingUser = new User(userId, "existingUsername", "hashedPassword");
        var userDto = new UserDto { Id = userId, UserName = "existingUsername" };

        _mockRepository.Setup(repo => repo.GetByUserName(userDto.UserName)).ReturnsAsync(existingUser);
        _mockRepository.Setup(repo => repo.Get(userDto.Id)).ReturnsAsync(existingUser);


        Func<Task> act = async () => await _userService.Update(userDto);

        await act.Should().NotThrowAsync<DomainException>();
        _mockRepository.Verify(repo => repo.GetByUserName(userDto.UserName), Times.Once);
        _mockRepository.Verify(repo => repo.Update(existingUser), Times.Once);
        _mockRepository.Verify(repo => repo.Commit(), Times.Once);
    }

    [Fact]
    public async Task Update_WhenUserNotFound_ThrowsArgumentException()
    {
        var userDto = new UserDto { Id = Guid.NewGuid(), UserName = "nonExistingUser" };

        _mockRepository.Setup(repo => repo.Get(userDto.Id)).ReturnsAsync(() => null);

        Func<Task> act = async () => await _userService.Update(userDto);

        await act.Should().ThrowAsync<ArgumentException>().WithMessage("User not Found");
        _mockRepository.Verify(repo => repo.Get(userDto.Id), Times.Once);
        _mockRepository.Verify(repo => repo.Update(It.IsAny<User>()), Times.Never);
        _mockRepository.Verify(repo => repo.Commit(), Times.Never);
    }

    [Fact]
    public async Task Update_WhenValidUserDto_UpdatesUser()
    {
        var validPassword = "ValidPassword123*!@";
        var userDto = new UserDto { Id = Guid.NewGuid(), UserName = "updatedUserName", PasswordHash = validPassword, RepeatPassword = validPassword };
        var existingUser = new User(userDto.Id, "originalUserName", "hashedPassword");

        _mockRepository.Setup(repo => repo.Get(userDto.Id)).ReturnsAsync(existingUser);

        await _userService.Update(userDto);

        existingUser.UserName.Should().Be(userDto.UserName);
        _mockRepository.Verify(repo => repo.Get(userDto.Id), Times.Once);
        _mockRepository.Verify(repo => repo.Update(existingUser), Times.Once);
        _mockRepository.Verify(repo => repo.Commit(), Times.Once);
    }

    [Fact]
    public async Task UpdatePassword_WhenUserNotFound_ThrowsArgumentException()
    {
        var userDto = new UserDto { Id = Guid.NewGuid(), PasswordHash = "newPassword" };

        _mockRepository.Setup(repo => repo.Get(userDto.Id)).ReturnsAsync(() => null);

        Func<Task> act = async () => await _userService.UpdatePassword(userDto);

        await act.Should().ThrowAsync<ArgumentException>().WithMessage("User not Found");
        _mockRepository.Verify(repo => repo.Get(userDto.Id), Times.Once);
        _mockRepository.Verify(repo => repo.Update(It.IsAny<User>()), Times.Never);
        _mockRepository.Verify(repo => repo.Commit(), Times.Never);
    }

    [Fact]
    public async Task UpdatePassword_WhenValidUserDto_UpdatesPassword()
    {
        var validPassword = "ValidPassword123*!@";
        var userDto = new UserDto { Id = Guid.NewGuid(), PasswordHash = validPassword, RepeatPassword = validPassword };
        var existingUser = new User(userDto.Id, "existingUserName", "existingPassword");

        _mockRepository.Setup(repo => repo.Get(userDto.Id)).ReturnsAsync(existingUser);

        await _userService.UpdatePassword(userDto);

        _mockRepository.Verify(repo => repo.Get(userDto.Id), Times.Once);
        _mockRepository.Verify(repo => repo.Update(existingUser), Times.Once);
        _mockRepository.Verify(repo => repo.Commit(), Times.Once);
    }

     [Fact]
    public async Task UpdateRoles_WhenUserNotFound_ThrowsArgumentException()
    {
        var userDto = new UserDto { Id = Guid.NewGuid(), Roles = new List<string> { "Admin" } };

        _mockRepository.Setup(repo => repo.Get(userDto.Id)).ReturnsAsync(() => null);

        Func<Task> act = async () => await _userService.UpdateRoles(userDto);

        await act.Should().ThrowAsync<ArgumentException>().WithMessage("User not Found");
        _mockRepository.Verify(repo => repo.Get(userDto.Id), Times.Once);
        _mockRepository.Verify(repo => repo.Update(It.IsAny<User>()), Times.Never);
        _mockRepository.Verify(repo => repo.Commit(), Times.Never);
    }

    [Fact]
    public async Task UpdateRoles_WhenValidUserDto_UpdatesRoles()
    {
        var userDto = new UserDto { Id = Guid.NewGuid(), Roles = new List<string> { "Admin", "Moderator" } };
        var existingUser = new User(userDto.Id, "existingUserName", "existingPassword");

        _mockRepository.Setup(repo => repo.Get(userDto.Id)).ReturnsAsync(existingUser);

        await _userService.UpdateRoles(userDto);

        existingUser.Roles.Should().BeEquivalentTo(userDto.Roles);
        _mockRepository.Verify(repo => repo.Get(userDto.Id), Times.Once);
        _mockRepository.Verify(repo => repo.Update(existingUser), Times.Once);
        _mockRepository.Verify(repo => repo.Commit(), Times.Once);
    }
}
