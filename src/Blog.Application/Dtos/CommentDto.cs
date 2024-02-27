namespace Blog.Application;

public class CommentDto
{
    public Guid Id { get; set; }
    public string Author { get; set; }
    public string Content { get; set; }

    public CommentDto(string author, string content)
    {
        Author = author;
        Content = content;
    }

    public CommentDto()
    {
        
    }
}
