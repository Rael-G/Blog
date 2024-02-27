using Blog.Domain;

namespace Blog.Application;

public class PostDto
    (string title, string content, IEnumerable<Comment> comments)
{
    public Guid? Id { get; set; }
    public string Title { get; set; } = title;
    public string Content { get; set; } = content;
    public IEnumerable<Comment> Comments { get; set; } = comments;
}
