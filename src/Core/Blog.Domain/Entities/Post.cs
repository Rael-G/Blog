namespace Blog.Domain;

public class Post : BaseEntity
{
    /// <summary>
    /// Gets or sets the title of the post.
    /// </summary>
    public string Title { get => _title; set => _title = SetTitle(value); }
    
    /// <summary>
    /// Gets or sets the content of the post.
    /// </summary>
    public string Content { get => _content; set => _content = SetContent(value); }

    /// <summary>
    /// Gets the comments associated with the post.
    /// </summary>
    public IEnumerable<Comment> Comments { get => _comments; private set => _comments = SetComments(value); }

    private string _title = string.Empty;
    private string _content = string.Empty;
    private IEnumerable<Comment> _comments = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="Post"/> class with the specified title, content, and comments.
    /// </summary>
    /// <param name="title">The title of the post.</param>
    /// <param name="content">The content of the post.</param>
    /// <param name="comments">Optional comments associated with the post.</param>
    public Post(string title, string content, IEnumerable<Comment>? comments = null)
    {
        Title = title;
        Content = content;
        Comments = comments?? [];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Post"/> class with the specified identifier, title, content, and comments.
    /// </summary>
    /// <param name="id">The identifier of the post.</param>
    /// <param name="title">The title of the post.</param>
    /// <param name="content">The content of the post.</param>
    /// <param name="comments">Optional comments associated with the post.</param>
    public Post(Guid id, string title, string content, IEnumerable<Comment>? comments) 
        : base(id)
    {
        Title = title;
        Content = content;
        Comments = comments?? [];
    }

    private Post() { }

    /// <summary>
    /// Validates the title of the post.
    /// </summary>
    /// <param name="title">The title to validate.</param>
    /// <returns>The validated title.</returns>
    private static string SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title must contain a value", nameof(title));

        if (title.Length > 256)
            throw new ArgumentException("Title max length is 256", nameof(title));

        return title;
    }

    /// <summary>
    /// Validates the content of the post.
    /// </summary>
    /// <param name="content">The content to validate.</param>
    /// <returns>The validated content.</returns>
    private static string SetContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Content must contain a value", nameof(content));

        return content;
    }

    /// <summary>
    /// Validates the comments associated with the post.
    /// </summary>
    /// <param name="comments">The comments to validate.</param>
    /// <returns>The validated comments.</returns>
    private static IEnumerable<Comment> SetComments(IEnumerable<Comment> comments)
        => comments ?? [];

}