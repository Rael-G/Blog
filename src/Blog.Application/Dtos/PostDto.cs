namespace Blog.Application;

public class PostDto : IDto
{
    public Guid Id { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime ModifiedTime { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public IEnumerable<CommentDto> Comments { get; set; } = [];
    public IEnumerable<TagDto> Tags { get; set; } = [];
}