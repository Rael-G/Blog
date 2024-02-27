namespace Blog.Domain;

public class Comment
    : BaseEntity
{
    /// <summary>
    /// Gets or sets the author of the comment.
    /// </summary>
    public string Author { get => _author; set => _author = SetAuthor(value); }

    /// <summary>
    /// Gets or sets the content of the comment.
    /// </summary>
    public string Content { get => _content; set => _content = SetContent(value); }

    private string _author = string.Empty;
    private string _content = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="Comment"/> class with specified identifier, author, and content.
    /// </summary>
    /// <param name="id">The identifier of the comment.</param>
    /// <param name="author">The author of the comment.</param>
    /// <param name="content">The content of the comment.</param>
    public Comment(Guid id, string author, string content) : base(id)
    { 
        Author = author;
        Content = content;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Comment"/> class with specified author and content.
    /// </summary>
    /// <param name="author">The author of the comment.</param>
    /// <param name="content">The content of the comment.</param>
    public Comment(string author, string content)
    { 
        Author = author;
        Content = content;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Comment"/> class.
    /// </summary>
    private Comment() { }

    /// <summary>
    /// Validates the author of the comment.
    /// </summary>
    /// <param name="author">The author to validate.</param>
    /// <returns>The valid author.</returns>
    private static string SetAuthor(string author)
    {
        if (string.IsNullOrWhiteSpace(author))
            throw new ArgumentException("Author must contain a value", nameof(author));

        if (author.Length > 256)
            throw new ArgumentException("Author max length is 256", nameof(author));

        return author;
    }

    /// <summary>
    /// Validates the content of the comment.
    /// </summary>
    /// <param name="content">The content to validate.</param>
    /// <returns>The valid content.</returns>
    private static string SetContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Content must contain a value", nameof(content));

        if (content.Length > 256)
            throw new ArgumentException("Content max length is 512", nameof(content));

        return content;
    }
}
