namespace Blog.Domain.Entities;

public class Post
    (string title, string content, IEnumerable<Comment>? comments) : BaseEntity
{
    public string Title { get; set; } = title;
    public string Content { get; set; } = content;
    public IEnumerable<Comment> Comments { get; set; } = comments??= [];

}