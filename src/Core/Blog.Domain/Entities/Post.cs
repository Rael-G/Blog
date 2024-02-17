namespace Blog.Domain;

public class Post : BaseEntity
{
    public string Title { get; set; }
    public string Content { get; set; }
    public IEnumerable<Comment> Comments { get; private set; }

    public Post(string title, string content, IEnumerable<Comment>? comments) 
        : base(null)
    {
        Title = title;
        Content = content;
        Comments = comments?? [];
    }

    public Post(Guid id, string title, string content, IEnumerable<Comment>? comments) 
        : base(id)
    {
        Title = title;
        Content = content;
        Comments = comments?? [];
    }

}