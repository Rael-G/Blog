namespace Blog.Application;

public class TagDto : IDto
{
    public Guid Id { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime ModifiedTime { get; set; }
    public string Name { get; set; } = string.Empty;
    public IEnumerable<PostDto> Posts { get; set; } = [];
}