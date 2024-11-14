namespace Blog.Application;

public class UserSummaryDto : IDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
}
