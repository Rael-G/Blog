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
    /// <exception cref="ArgumentNullException">Thrown when title is null or empty,.</exception>
    /// <exception cref="ArgumentException">Thrown when title exceeds {TitleMaxLength} characters.</exception>
    public string Title { get => _title; set => _title = ValidateTitle(value); }

    /// <summary>
    /// Gets or sets the content of the post.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when the content is null or empty.</exception>
    public string Content { get => _content; set => _content = ValidateContent(value); }

    /// <summary>
    /// Gets the comments associated with the post.
    /// </summary>
    public IEnumerable<Comment> Comments { get; }

    /// <summary>
    /// Gets the relation of tags associated with the post.
    /// </summary>
    public IEnumerable<PostTag> Tags { get; }

    public User? User { get; }
    public Guid UserId { get; }

    private string _title = string.Empty;
    private string _content = string.Empty;

    /// <summary>
    /// Initializes a new instance of the post class with the specified <see cref="Title">, <see cref="Content">.
    /// </summary>
    /// <param name="id">The identifier of the post.</param>
    /// <param name="title">The <see cref="Title"> of the post.</param>
    /// <param name="content">The content of the post.</param>
    /// <exception cref="ArgumentException"></exception>
    public Post(Guid id, string title, string content, Guid userId)
        : base(id)
    {
        Title = title;
        Content = content;
        Comments = new List<Comment>();
        Tags = new List<PostTag>();
        UserId = userId;
    }

    private string ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentNullException($"{nameof(title)} must contain a value", nameof(title));

        if (title.Length > TitleMaxLength)
            throw new ArgumentException($"{nameof(title)} max length is {TitleMaxLength}", nameof(title));

        return title;
    }

    private string ValidateContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentNullException($"{nameof(content)} must contain a value", nameof(content));

        return content;
    }
}