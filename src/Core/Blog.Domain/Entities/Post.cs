namespace Blog.Domain;

public class Post : BaseEntity
{
    public string Title { get => _title; set => _title = SetTitle(value); }
    public string Content { get => _content; set => _content = SetContent(value); }
    public IEnumerable<Comment> Comments { get => _comments; private set => _comments = SetComments(value); }

    private string _title;
    private string _content;
    private IEnumerable<Comment> _comments;

    public Post(string title, string content, IEnumerable<Comment>? comments)
    {
        Title = title;
        Content = content;
        Comments = comments;
    }

    public Post(Guid id, string title, string content, IEnumerable<Comment>? comments) 
        : base(id)
    {
        Title = title;
        Content = content;
        Comments = comments;
    }

    private Post() { }

    private static string SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title must contain a value", nameof(title));

        if (title.Length > 256)
            throw new ArgumentException("Title max length is 256", nameof(title));

        return title;
    }

    private static string SetContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Content must contain a value", nameof(content));

        return content;
    }

    private static IEnumerable<Comment> SetComments(IEnumerable<Comment> comments)
        => comments ?? [];

}