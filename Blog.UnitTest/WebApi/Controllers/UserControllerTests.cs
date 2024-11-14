using System.Security.Claims;
using Blog.Application;
using Blog.Domain;
using Blog.WebApi;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Blog.UnitTest.WebApi.Controllers;

public class UserControllerTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly UsersController _userController;

    private UserDto _userDto;
    private List<UserDto> _userDtos;
    private SigninInputModel _signinInput;

    public UserControllerTests()
    {
        _mockUserService = new Mock<IUserService>();
        _userController = new UsersController(_mockUserService.Object);

        _userDto = new(){ Id = Guid.NewGuid(), UserName = "CoolUsername" };
        _userDtos = new List<UserDto>
        {
            new() { Id = Guid.NewGuid(), UserName = "User1" },
            new() { Id = Guid.NewGuid(), UserName = "User2" }
        };

        _signinInput = new SigninInputModel
            {
                UserName = "NewUser",
                Password = "Password123",
                RepeatPassword = "Password123"
            };
    }

    private void SetUserClaims(Guid userId, bool isAdmin = false)
    {
        var claims = new List<Claim>
        {
            new("UserId", userId.ToString()),
            new(ClaimTypes.Role, isAdmin ? "admin" : "user")
        };

        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var principal = new ClaimsPrincipal(identity);
        _userController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };
    }

    [Fact]
    public async Task Get_UserNotFound_ReturnsNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        SetUserClaims(userId, true);

        _mockUserService.Setup(service => service.Get(userId)).ReturnsAsync(() => null);

        // Act
        var result = await _userController.Get(userId);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult!.Value.Should().BeEquivalentTo(new { Id = userId });
    }

    [Fact]
    public async Task Get_UserFound_ReturnsOk()
    {
        // Arrange
        SetUserClaims(_userDto.Id);

        _mockUserService.Setup(service => service.Get(_userDto.Id)).ReturnsAsync(_userDto);

        // Act
        var result = await _userController.Get(_userDto.Id);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(new UserOutputModel(_userDto));
    }

    [Fact]
    public async Task Get_UserForbidden_ReturnsForbid()
    {
        // Arrange
        var otherUserId = Guid.NewGuid();
        SetUserClaims(otherUserId);

        _mockUserService.Setup(service => service.Get(_userDto.Id)).ReturnsAsync(_userDto);

        // Act
        var result = await _userController.Get(_userDto.Id);

        // Assert
        result.Should().BeOfType<ForbidResult>();
    }

    [Fact]
    public async Task Get_UserAdmin_ReturnsOk()
    {
        // Arrange
        var adminId = Guid.NewGuid();
        SetUserClaims(adminId, isAdmin: true);

        _mockUserService.Setup(service => service.Get(_userDto.Id)).ReturnsAsync(_userDto);

        // Act
        var result = await _userController.Get(_userDto.Id);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(new UserOutputModel(_userDto));
    }

        [Fact]
    public async Task GetPage_UserNotFound_ReturnsNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var page = 1;
        _mockUserService.Setup(service => service.GetUserPage(userId, page)).ReturnsAsync(() => null);

        // Act
        var result = await _userController.GetPage(userId, page);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult!.Value.Should().BeEquivalentTo(new { Id = userId });
    }

    [Fact]
    public async Task GetPage_UserFound_ReturnsOk()
    {
        // Arrange
        var userId = _userDto.Id;
        var page = 1;
        _mockUserService.Setup(service => service.GetUserPage(userId, page)).ReturnsAsync(_userDto);

        // Act
        var result = await _userController.GetPage(userId, page);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(new UserOutputModel(_userDto));
    }

    [Fact]
    public async Task GetPageCount_UserNotFound_ReturnsNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _mockUserService.Setup(service => service.Get(userId)).ReturnsAsync(() => null);

        // Act
        var result = await _userController.GetPageCount(userId);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult!.Value.Should().BeEquivalentTo(new { Id = userId });
    }

    [Fact]
    public async Task GetPageCount_UserFound_ReturnsOk()
    {
        // Arrange
        var userId = _userDto.Id;
        var pageCount = 5;
        _mockUserService.Setup(service => service.Get(userId)).ReturnsAsync(_userDto);
        _mockUserService.Setup(service => service.GetPageCount(userId)).ReturnsAsync(pageCount);

        // Act
        var result = await _userController.GetPageCount(userId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(pageCount);
    }

    [Fact]
    public async Task GetByUserName_UserNotFound_ReturnsNotFound()
    {
        // Arrange
        var username = "NonExistentUser";
        _mockUserService.Setup(service => service.GetByUserName(username)).ReturnsAsync(() => null!);

        // Act
        var result = await _userController.GetByUserName(username);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult!.Value.Should().BeEquivalentTo(new { UserName = username });
    }

    [Fact]
    public async Task GetByUserName_UserFound_ReturnsOk()
    {
        // Arrange
        SetUserClaims(_userDto.Id);

        _mockUserService.Setup(service => service.GetByUserName(_userDto.UserName)).ReturnsAsync(_userDto);

        // Act
        var result = await _userController.GetByUserName(_userDto.UserName);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(new UserOutputModel(_userDto));
    }

    [Fact]
    public async Task GetByUserName_UserForbidden_ReturnsForbid()
    {
        // Arrange
        var otherUserId = Guid.NewGuid();
        SetUserClaims(otherUserId);

        _mockUserService.Setup(service => service.GetByUserName(_userDto.UserName)).ReturnsAsync(_userDto);

        // Act
        var result = await _userController.GetByUserName(_userDto.UserName);

        // Assert
        result.Should().BeOfType<ForbidResult>();
    }

    [Fact]
    public async Task GetByUserName_UserAdmin_ReturnsOk()
    {
        // Arrange
        var adminId = Guid.NewGuid();
        SetUserClaims(adminId, isAdmin: true);

        _mockUserService.Setup(service => service.GetByUserName(_userDto.UserName)).ReturnsAsync(_userDto);

        // Act
        var result = await _userController.GetByUserName(_userDto.UserName);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(new UserOutputModel(_userDto));
    }

    [Fact]
    public async Task GetAll_AdminUser_ReturnsOk()
    {
        // Arrange
        var adminId = Guid.NewGuid();
        SetUserClaims(adminId, isAdmin: true);

        _mockUserService.Setup(service => service.GetAll()).ReturnsAsync(_userDtos);

        // Act
        var result = await _userController.GetAll();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(UserOutputModel.MapRange(_userDtos));
    }

    [Fact]
    public async Task Post_ValidInput_ReturnsCreated()
    {
        // Arrange
        var userDto = _signinInput.InputToDto();
        userDto.Id = Guid.NewGuid();

        _mockUserService.Setup(service => service.Create(It.IsAny<UserDto>()))
                        .Returns(Task.CompletedTask);

        // Act
        var result = await _userController.Post(_signinInput);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result as CreatedAtActionResult;
        createdResult!.ActionName.Should().Be(nameof(_userController.Get));
        createdResult.RouteValues!["id"].Should().BeAssignableTo<Guid>();
        var createResultId = createdResult.RouteValues!["id"] as Guid?;
        var returnedUser = createdResult.Value as UserOutputModel;
        returnedUser!.Id.Should().Be(createResultId.ToString());
        returnedUser.UserName.Should().Be(userDto.UserName);
    }

    [Fact]
    public async Task Post_InvalidInput_ReturnsBadRequest()
    {
        // Arrange
        _userController.ModelState.AddModelError("UserName", "Required");
        _signinInput.UserName = "";

        // Act
        var result = await _userController.Post(_signinInput);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.Value.Should().BeEquivalentTo(new SerializableError(_userController.ModelState));
    }

    [Fact]
    public async Task Post_DomainException_ReturnsBadRequest()
    {
        // Arrange
        var userDto = _signinInput.InputToDto();
        var domainExceptionMessage = "User already exists.";

        _mockUserService.Setup(service => service.Create(It.IsAny<UserDto>()))
                        .ThrowsAsync(new DomainException(domainExceptionMessage));

        // Act
        var result = await _userController.Post(_signinInput);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.Value.Should().BeEquivalentTo(new { Message = domainExceptionMessage });
    }

    [Fact]
    public async Task Put_ValidInput_ReturnsNoContent()
    {
        // Arrange
        var userId = Guid.NewGuid();
        SetUserClaims(userId);

        var input = new UserInputModel { UserName = "UpdatedUser" };
        var userDto = new UserDto { Id = userId, UserName = "OldUser" };

        _mockUserService.Setup(service => service.Get(userId)).ReturnsAsync(userDto);
        _mockUserService.Setup(service => service.Update(It.IsAny<UserDto>())).Returns(Task.CompletedTask);

        // Act
        var result = await _userController.Put(userId, input);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Put_InvalidInput_ReturnsBadRequest()
    {
        // Arrange
        var userId = Guid.NewGuid();
        SetUserClaims(userId);

        _userController.ModelState.AddModelError("UserName", "Required");

        var input = new UserInputModel { UserName = "" };

        // Act
        var result = await _userController.Put(userId, input);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.Value.Should().BeEquivalentTo(new SerializableError(_userController.ModelState));
    }

    [Fact]
    public async Task Put_UserNotFound_ReturnsNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        SetUserClaims(userId);

        var input = new UserInputModel { UserName = "UpdatedUser" };

        _mockUserService.Setup(service => service.Get(userId)).ReturnsAsync(() => null);

        // Act
        var result = await _userController.Put(userId, input);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult!.Value.Should().BeEquivalentTo(new { Id = userId });
    }

    [Fact]
    public async Task Put_DomainException_ReturnsBadRequest()
    {
        // Arrange
        var userId = Guid.NewGuid();
        SetUserClaims(userId);

        var input = new UserInputModel { UserName = "UpdatedUser" };
        var userDto = new UserDto { Id = userId, UserName = "OldUser" };
        var domainExceptionMessage = "Some domain error occurred.";

        _mockUserService.Setup(service => service.Get(userId)).ReturnsAsync(userDto);
        _mockUserService.Setup(service => service.Update(It.IsAny<UserDto>()))
                        .ThrowsAsync(new DomainException(domainExceptionMessage));

        // Act
        var result = await _userController.Put(userId, input);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.Value.Should().BeEquivalentTo(new { Message = domainExceptionMessage });
    }

    [Fact]
    public async Task Put_UserForbidden_ReturnsForbid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();
        SetUserClaims(otherUserId);

        var input = new UserInputModel { UserName = "UpdatedUser" };

        _mockUserService.Setup(service => service.Get(userId)).ReturnsAsync(new UserDto { Id = userId });

        // Act
        var result = await _userController.Put(userId, input);

        // Assert
        result.Should().BeOfType<ForbidResult>();
    }

    [Fact]
    public async Task ResetPassword_ValidInput_ReturnsNoContent()
    {
        // Arrange
        var userId = Guid.NewGuid();
        SetUserClaims(userId);

        var input = new ResetPasswordInputModel { Password = "NewPassword123", RepeatPassword = "NewPassword123" };
        var userDto = new UserDto { Id = userId, UserName = "OldUser" };

        _mockUserService.Setup(service => service.Get(userId)).ReturnsAsync(userDto);
        _mockUserService.Setup(service => service.UpdatePassword(It.IsAny<UserDto>())).Returns(Task.CompletedTask);

        // Act
        var result = await _userController.ResetPassword(userId, input);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task ResetPassword_InvalidInput_ReturnsBadRequest()
    {
        // Arrange
        var userId = Guid.NewGuid();
        SetUserClaims(userId);

        _userController.ModelState.AddModelError("Password", "Required");

        var input = new ResetPasswordInputModel { Password = "" };

        // Act
        var result = await _userController.ResetPassword(userId, input);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.Value.Should().BeEquivalentTo(new SerializableError(_userController.ModelState));
    }

    [Fact]
    public async Task ResetPassword_UserNotFound_ReturnsNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        SetUserClaims(userId);

        var input = new ResetPasswordInputModel { Password = "NewPassword123", RepeatPassword = "NewPassword123" };

        _mockUserService.Setup(service => service.Get(userId)).ReturnsAsync(() => null);

        // Act
        var result = await _userController.ResetPassword(userId, input);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult!.Value.Should().BeEquivalentTo(new { Id = userId });
    }

    [Fact]
    public async Task ResetPassword_DomainException_ReturnsBadRequest()
    {
        // Arrange
        var userId = Guid.NewGuid();
        SetUserClaims(userId);

        var input = new ResetPasswordInputModel { Password = "NewPassword123" , RepeatPassword = "NewPassword123" };
        var userDto = new UserDto { Id = userId, UserName = "OldUser" };
        var domainExceptionMessage = "Some domain error occurred.";

        _mockUserService.Setup(service => service.Get(userId)).ReturnsAsync(userDto);
        _mockUserService.Setup(service => service.UpdatePassword(It.IsAny<UserDto>()))
                        .ThrowsAsync(new DomainException(domainExceptionMessage));

        // Act
        var result = await _userController.ResetPassword(userId, input);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.Value.Should().BeEquivalentTo(new { Message = domainExceptionMessage });
    }

    [Fact]
    public async Task ResetPassword_UserForbidden_ReturnsForbid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();
        SetUserClaims(otherUserId);

        var input = new ResetPasswordInputModel { Password = "NewPassword123", RepeatPassword = "NewPassword123" };

        _mockUserService.Setup(service => service.Get(userId)).ReturnsAsync(new UserDto { Id = userId });

        // Act
        var result = await _userController.ResetPassword(userId, input);

        // Assert
        result.Should().BeOfType<ForbidResult>();
    }

    [Fact]
    public async Task SetRoles_ValidInput_ReturnsNoContent()
    {
        // Arrange
        var userId = Guid.NewGuid();
        SetUserClaims(userId, isAdmin: true);

        var roles = new[] { "admin", "user" };
        var userDto = new UserDto { Id = userId, UserName = "User", Roles = roles };

        _mockUserService.Setup(service => service.Get(userId)).ReturnsAsync(userDto);
        _mockUserService.Setup(service => service.UpdateRoles(It.IsAny<UserDto>())).Returns(Task.CompletedTask);

        // Act
        var result = await _userController.SetRoles(userId, roles);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task SetRoles_InvalidInput_ReturnsBadRequest()
    {
        // Arrange
        var userId = Guid.NewGuid();
        SetUserClaims(userId, isAdmin: true);

        _userController.ModelState.AddModelError("Roles", "Required");

        var roles = Array.Empty<string>();

        // Act
        var result = await _userController.SetRoles(userId, roles);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.Value.Should().BeEquivalentTo(new SerializableError(_userController.ModelState));
    }

    [Fact]
    public async Task SetRoles_UserNotFound_ReturnsNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        SetUserClaims(userId, isAdmin: true);

        var roles = new[] { "admin", "user" };

        _mockUserService.Setup(service => service.Get(userId)).ReturnsAsync(() => null);

        // Act
        var result = await _userController.SetRoles(userId, roles);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult!.Value.Should().BeEquivalentTo(new { Id = userId });
    }

    [Fact]
    public async Task SetRoles_DomainException_ReturnsBadRequest()
    {
        // Arrange
        var userId = Guid.NewGuid();
        SetUserClaims(userId, isAdmin: true);

        var roles = new[] { "admin", "user" };
        var userDto = new UserDto { Id = userId, UserName = "User", Roles = roles };
        var domainExceptionMessage = "Some domain error occurred.";

        _mockUserService.Setup(service => service.Get(userId)).ReturnsAsync(userDto);
        _mockUserService.Setup(service => service.UpdateRoles(It.IsAny<UserDto>()))
                        .ThrowsAsync(new DomainException(domainExceptionMessage));

        // Act
        var result = await _userController.SetRoles(userId, roles);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.Value.Should().BeEquivalentTo(new { Message = domainExceptionMessage });
    }

    [Fact]
    public async Task Delete_UserDeletesSelf_ReturnsNoContent()
    {
        SetUserClaims(_userDto.Id);

        _mockUserService.Setup(service => service.Delete(It.IsAny<UserDto>())).Returns(Task.CompletedTask);
        _mockUserService.Setup(service => service.Get(_userDto.Id)).ReturnsAsync(_userDto);

        // Act
        var result = await _userController.Delete(_userDto.Id);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Delete_AdminDeletesUser_ReturnsNoContent()
    {
        // Arrange
        var adminId = Guid.NewGuid();
        SetUserClaims(adminId, isAdmin: true);

        _mockUserService.Setup(service => service.Delete(It.IsAny<UserDto>())).Returns(Task.CompletedTask);
        _mockUserService.Setup(service => service.Get(_userDto.Id)).ReturnsAsync(_userDto);

        // Act
        var result = await _userController.Delete(_userDto.Id);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Delete_UserNotFound_ReturnsNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        SetUserClaims(userId);

        _mockUserService.Setup(service => service.Delete(It.IsAny<UserDto>())).ThrowsAsync(new DomainException(userId.ToString()));

        // Act
        var result = await _userController.Delete(userId);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult!.Value.Should().BeEquivalentTo(new { Id = userId });
    }

    [Fact]
    public async Task Delete_UserForbidden_ReturnsForbid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();
        SetUserClaims(otherUserId);

        // Act
        var result = await _userController.Delete(userId);

        // Assert
        result.Should().BeOfType<ForbidResult>();
    }
}
