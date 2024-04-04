namespace Blog.Application;

public class PostDto : IDto
{
    public Guid Id { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime ModifiedTime { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public IEnumerable<CommentDto> Comments { get; set; }
    public IEnumerable<TagDto> Tags { get; set; }

    public PostDto(Guid id, string title, string content, IEnumerable<CommentDto> comments, IEnumerable<TagDto> tags)
    {
        Id = id;
        Title = title;
        Content = content;
        Comments = comments;
        Tags = tags;
    }
}
