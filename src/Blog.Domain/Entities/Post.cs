namespace Blog.Domain;

/// <summary>
/// Represents a post on the blog.
/// </summary>
public class Post : BaseEntity
{
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
    /// Gets the tags associated with the post.
    /// </summary>
    public IEnumerable<Tag> Tags { get; }

    private const int TitleMaxLength = 100;
    private const int TagsMinLength = 1;

    private string _title = "";
    private string _content = "";

    /// <summary>
    /// Initializes a new instance of the post class with the specified <see cref="Title">, <see cref="Content">.
    /// </summary>
    /// <param name="id">The identifier of the post.</param>
    /// <param name="title">The <see cref="Title"> of the post.</param>
    /// <param name="content">The content of the post.</param>
    /// <exception cref="ArgumentException"></exception>
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

    private IEnumerable<Tag> ValidateTags(IEnumerable<Tag> tags)
    {
        if (tags.Count() < TagsMinLength)
            throw new ArgumentException($"{nameof(tags)} minimum length is {TagsMinLength}", nameof(tags));

        return tags;
    }
}