using Blog.Domain;

namespace Blog.UnitTest.Domain.Entities;

public class UserTests
{
    private User _user;

    public UserTests()
    {
        var id = Guid.NewGuid();
        var username = "TestUsername";
        var passwordHash = Guid.NewGuid().ToString();
        var roles = new string[]{"admin", "moderator"};
        _user = new User(id, username, passwordHash, roles);
    }

    [Fact]
    public void User_Initialization_WithValidValues_Success()
    {
        var user = new User(_user.Id, _user.UserName, _user.PasswordHash, _user.Roles);

        Assert.Equal(_user.UserName, user.UserName);
        Assert.Equal(_user.PasswordHash, user.PasswordHash);
        Assert.Equal(_user.Roles, user.Roles);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void User_PasswordRegex_EmptyOrWhiteSpace_ThrowsDomainException(string password)
    {
       Assert.False(User.PasswordRegex().Match(password).Success);
    }

    [Theory]
    [InlineData("Aabc1!a")]
    [InlineData("aBc1!a")]
    [InlineData("A1!a")]
    public void User_PasswordRegex_LessThanEight_ThrowsDomainException(string password)
    {
        Assert.False(User.PasswordRegex().Match(password).Success);
    }

    [Theory]
    [InlineData("aaabc1!a")]
    [InlineData("ab1!defg")]
    [InlineData("12345!abc")]
    public void User_PasswordRegex_WithoutCapitalLetter_ThrowsDomainException(string password)
    {
        Assert.False(User.PasswordRegex().Match(password).Success);
    }

    [Theory]
    [InlineData("AABB1!BV")]
    [InlineData("ABCDE1!@@2")]
    [InlineData("1234!XYZ")]
    public void User_PasswordRegex_WithoutLowerCase_ThrowsDomainException(string password)
    {
        Assert.False(User.PasswordRegex().Match(password).Success);
    }

    [Theory]
    [InlineData("AAbbb1adfBV")]
    [InlineData("Abcdef1G2")]
    [InlineData("123AbVcde")]
    public void User_PasswordRegex_WithoutSymbol_ThrowsDomainException(string password)
    {
        Assert.False(User.PasswordRegex().Match(password).Success);
    }

    [Theory]
    [InlineData("AAbbb!adfBV")]
    [InlineData("Abcdef!GhI")]
    [InlineData("!Abcdefgh")]
    public void User_PasswordRegex_WithoutNumber_ThrowsDomainException(string password)
    {
        Assert.False(User.PasswordRegex().Match(password).Success);
    }

    [Theory]
    [InlineData("Aab1@asd")]
    [InlineData("Passw0rd!")]
    [InlineData("1A2b3@4c")]
    [InlineData("Valid1Password@")]
    public void User_PasswordRegex_WithCapitalLowerNumberSymbolAndMoreThanEight_Success(string password)
    {
        Assert.True(User.PasswordRegex().Match(password).Success);
    }

    [Theory]
    [InlineData("a")]
    [InlineData("ab")]
    [InlineData("a-")]
    public void UserNameRegex_LessThanThreeCharacters_ThrowsDomainException(string username)
    {
        Assert.Throws<DomainException>(() => _user.UserName = username);
    }

    [Theory]
    [InlineData("abcdefghijklmnopqrstuvwxyz0123456789")]
    [InlineData("aaavvvvvvvvvvvvvvvvvccccccccccccccccccccccccddd")]
    [InlineData("user-name_123456789012345678900")]
    public void UserNameRegex_MoreThanThirtyCharacters_ThrowsDomainException(string username)
    {
        Assert.Throws<DomainException>(() => _user.UserName = username);
    }

    [Theory]
    [InlineData("abc!")]
    [InlineData("username@123")]
    [InlineData("user#name")]
    [InlineData("name space")]
    public void UserNameRegex_ContainsInvalidCharacters_ThrowsDomainException(string username)
    {
        Assert.Throws<DomainException>(() => _user.UserName = username);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("user_name")]
    [InlineData("User-Name")]
    [InlineData("username123")]
    [InlineData("ValidUserName_123")]
    public void UserNameRegex_ValidUserName_Success(string username)
    {
        _user.UserName = username;
        Assert.Equal(_user.UserName, username);
    }
}
