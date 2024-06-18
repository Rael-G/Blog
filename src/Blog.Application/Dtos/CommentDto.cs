namespace Blog.Application;

public class CommentDto : IDto
{
    public Guid Id { get; set; }
    public DateTime CreatedTime { get; set; }
    public string Author { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Guid PostId { get; set; }
}