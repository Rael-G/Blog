namespace Blog.Domain;

/// <summary>
/// Represents a post on the blog.
/// </summary>
public class Post : BaseEntity
{
    /// <summary>
    /// Maximum length allowed for the title of the post.
    /// </summary>
    public const int TitleMaxLength = 100;

    /// <summary>
    /// Gets or sets the title of the post.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when <see cref="Title"/> is null, empty, or exceeds 256 characters.</exception>
    public string Title { get => _title; set => _title = ValidateTitle(value); }

    /// <summary>
    /// Gets or sets the content of the post.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when <see cref="Content"/> is null or empty.</exception>
    public string Content { get => _content; set => _content = ValidateContent(value); }

    /// <summary>
    /// Gets the comments associated with the post.
    /// </summary>
    public IEnumerable<Comment> Comments { get; }

    /// <summary>
    /// Gets the tags associated with the post.
    /// </summary>
    public IEnumerable<Tag> Tags { get; }

    private string _title = "";
    private string _content = "";

    /// <summary>
    /// Initializes a new instance of the <see cref="Post"/> class with the specified identifier, title, content, created time, and update time.
    /// </summary>
    /// <param name="id">The identifier of the post.</param>
    /// <param name="title">The title of the post.</param>
    /// <param name="content">The content of the post.</param>
    public Post(Guid id, string title, string content)
        : base(id)
    {
        Title = title;
        Content = content;
        Comments = new List<Comment>();
        Tags = new List<Tag>();
    }

    private string ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException($"{nameof(title)} must contain a value", nameof(title));

        if (title.Length > TitleMaxLength)
            throw new ArgumentException($"{nameof(title)} max length is {TitleMaxLength}", nameof(title));

        return title;
    }

    private string ValidateContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException($"{nameof(content)} must contain a value", nameof(content));
        
        return content;
    }
}