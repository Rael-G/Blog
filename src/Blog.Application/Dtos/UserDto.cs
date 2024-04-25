namespace Blog.Application;

public class UserDto : IDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string? PasswordHash { get; set; } = string.Empty;
    public string? RepeatPassword { get; set; } = string.Empty;
    public IEnumerable<string>? Roles { get; set; }
    public IEnumerable<PostDto> Posts{ get; } = [];
}
