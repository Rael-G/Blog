namespace Blog.Application;

public class PostDto
{
    public Guid? Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public IEnumerable<CommentDto> Comments { get; set; }

    public PostDto(string title, string content, IEnumerable<CommentDto> comments)
    {
        Title = title;
        Content = content;
        Comments = comments;
    }

    public PostDto()
    {
        
    }
}
