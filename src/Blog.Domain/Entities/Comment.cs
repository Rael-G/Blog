namespace Blog.Domain;

/// <summary>
/// Represents a comment on a blog post.
/// </summary>
public class Comment : BaseEntity
{
    /// <summary>
    /// Maximum length allowed for the author of the comment.
    /// </summary>
    public const int AuthorMaxLength = 100;

    /// <summary>
    /// Maximum length allowed for the content of the comment.
    /// </summary>
    public const int ContentMaxLength = 255;

    /// <summary>
    /// Gets or sets the author of the comment.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when <see cref="Author"/> is null, empty, or exceeds 256 characters.</exception>
    public string Author { get => _author; set => _author = ValidateAuthor(value); }

    /// <summary>
    /// Gets or sets the content of the comment.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when <see cref="Content"/> is null or empty.</exception>
    public string Content { get => _content; set => _content = ValidateContent(value); }

    /// <summary>
    /// Gets the associated post of the comment.
    /// </summary>
    public Post? Post { get; private set; }

    /// <summary>
    /// Gets the identifier of the associated post.
    /// </summary>
    public Guid PostId { get; }

    private string _author = "";
    private string _content = "";

    /// <summary>
    /// Initializes a new instance of the <see cref="Comment"/> class with the specified identifier, author, content, created time, update time and associated post.
    /// </summary>
    /// <param name="id">The identifier of the comment.</param>
    /// <param name="author">The author of the comment.</param>
    /// <param name="content">The content of the comment.</param>
    /// <param name="post">The associated post of the comment.</param>
    public Comment(Guid id, string author, string content, Guid postId) : base(id)
    {
        Author = author;
        Content = content;
        PostId = postId;
    }

    private static string ValidateAuthor(string author)
    {
        if (string.IsNullOrWhiteSpace(author))
            throw new ArgumentException($"{nameof(author)} must contain a value", nameof(author));

        if (author.Length > AuthorMaxLength)
            throw new ArgumentException($"{nameof(author)} max length is {AuthorMaxLength}", nameof(author));

        return author;
    }

    private static string ValidateContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException($"{nameof(content)} must contain a value", nameof(content));

        if (content.Length > ContentMaxLength)
            throw new ArgumentException($"{nameof(content)} max length is {ContentMaxLength}", nameof(content));

        return content;
    }
}