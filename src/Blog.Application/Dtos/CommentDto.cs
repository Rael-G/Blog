namespace Blog.Application;

public class CommentDto
    (string author, string content)
{
    public Guid? Id { get; set; }
    public string Author { get; set; } = author;
    public string Content { get; set; } = content;
}
