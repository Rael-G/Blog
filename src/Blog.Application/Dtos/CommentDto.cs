namespace Blog.Application;

public class CommentDto
{
    public Guid Id { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime ModifiedTime { get; set; }
    public string Author { get; set; }
    public string Content { get; set; }
    public Guid PostId { get; set; }

    public CommentDto(Guid id, string author, string content, Guid postId)
    {
        Id = id;
        Author = author;
        Content = content;
        PostId = postId;
    }
}
