using System.Text.RegularExpressions;

namespace Blog.Domain;

public partial class User : BaseEntity
{
    public string UserName { get => _userName; set => _userName = ValidateUsername(value); }
    public string PasswordHash { get; set; }
    public IEnumerable<string> Roles { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public IEnumerable<Post> Posts{ get; }

    private string _userName = string.Empty;

    public User(Guid id, string userName, string passwordHash, IEnumerable<string>? roles = null) : base(id)
    {
        UserName = userName;
        PasswordHash = passwordHash;
        Roles = roles?? new List<string>();
        Posts = new List<Post>();
    }

    private static string ValidateUsername(string username)
    {
        if (!UserNameRegex().Match(username).Success)
            throw new DomainException("Username must be alphanumeric and between 3 and 30 characters long");

        return username;
    }

    [GeneratedRegex(@"^([a-zA-Z0-9-_]){3,30}$")]
    private static partial Regex UserNameRegex();
}
