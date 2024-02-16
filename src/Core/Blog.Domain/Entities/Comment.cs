namespace Blog.Domain.Entities;

public class Comment
    (string author, string content) : BaseEntity
{
    public string Author { get; set; } = author;
    public string Content { get; set; } = content;
}
