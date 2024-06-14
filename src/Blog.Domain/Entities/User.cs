using System.Text.RegularExpressions;

namespace Blog.Domain;

/// <summary>
/// Represents a user in the blog domain.
/// </summary>
public partial class User : BaseEntity
{
    /// <summary>
    /// Gets or sets the username of the user. The username must be alphanumeric and between 3 and 30 characters long.
    /// </summary>
    /// <exception cref="DomainException">Thrown when the username does not match the required pattern.</exception>
    public string UserName 
    { 
        get => _userName; 
        set => _userName = ValidateUsername(value); 
    }

    /// <summary>
    /// Gets or sets the password hash of the user.
    /// </summary>
    public string PasswordHash { get; set; }

    /// <summary>
    /// Gets or sets the roles of the user.
    /// </summary>
    public IEnumerable<string> Roles { get; set; }

    /// <summary>
    /// Gets or sets the refresh token of the user.
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Gets or sets the refresh token expiry time of the user.
    /// </summary>
    public DateTime? RefreshTokenExpiryTime { get; set; }

    private string _userName = string.Empty;

    /// <summary>
    /// Gets the posts created by the user.
    /// </summary>
    public IEnumerable<Post> Posts { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <param name="userName">The username of the user.</param>
    /// <param name="passwordHash">The password hash of the user.</param>
    /// <param name="roles">The roles assigned to the user. Defaults to an empty list if not provided.</param>
    /// <exception cref="DomainException">Thrown when the username does not match the required pattern.</exception>
    public User(Guid id, string userName, string passwordHash, IEnumerable<string>? roles = null) 
        : base(id)
    {
        UserName = userName;
        PasswordHash = passwordHash;
        Roles = roles ?? new List<string>();
        Posts = new List<Post>();
    }

    /// <summary>
    /// Validates the username to ensure it matches the required pattern.
    /// </summary>
    /// <param name="username">The username to validate.</param>
    /// <returns>The validated username.</returns>
    /// <exception cref="DomainException">Thrown when the username does not match the required pattern.</exception>
    private static string ValidateUsername(string username)
    {
        if (!UserNameRegex().Match(username).Success)
            throw new DomainException("Username must be alphanumeric and between 3 and 30 characters long");

        return username;
    }

    /// <summary>
    /// Gets the regular expression used to validate passwords.
    /// The password must be at least 8 characters long and contain at least one digit, one uppercase letter,
    /// one lowercase letter, and one special character.
    /// </summary>
    /// <returns>The password validation regular expression.</returns>
    [GeneratedRegex(@"^(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?=.*[^\w\d\s:])([^\s]){8,}$")]
    public static partial Regex PasswordRegex();

    /// <summary>
    /// Gets the regular expression used to validate usernames.
    /// The username must be alphanumeric and between 3 and 30 characters long.
    /// </summary>
    /// <returns>The username validation regular expression.</returns>
    [GeneratedRegex(@"^([a-zA-Z0-9-_]){3,30}$")]
    private static partial Regex UserNameRegex();
}
