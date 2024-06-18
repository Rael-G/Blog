namespace Blog.Application;

public class UserDto : IDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string? PasswordHash { get; set; } = null;
    public string? RepeatPassword { get; set; } = null;
    public IEnumerable<string>? Roles { get; set; } = null;
    public IEnumerable<PostDto> Posts{ get; set; } = [];
}
