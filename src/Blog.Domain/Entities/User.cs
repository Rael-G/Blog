namespace Blog.Domain;

public class User : BaseEntity
{
    public User(Guid id, string userName, string passwordHash, IEnumerable<string>? roles = null) : base(id)
    {
        UserName = userName;
        PasswordHash = passwordHash;
        Roles = roles?? new List<string>();
        Posts = new List<Post>();
    }

    public string UserName { get; set; }
    public string PasswordHash { get; set; }
    public IEnumerable<string> Roles { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public IEnumerable<Post> Posts{ get; }
}
