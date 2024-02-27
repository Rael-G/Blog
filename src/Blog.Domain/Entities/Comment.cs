using System.Diagnostics.CodeAnalysis;

namespace Blog.Domain;

public class Comment
    : BaseEntity
{
    /// <summary>
    /// Gets or sets the author of the comment.
    /// </summary>
    public string Author { get; set; }

    /// <summary>
    /// Gets or sets the content of the comment.
    /// </summary>
    public string Content { get; set; }

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
    /// Private constructor for entity framework use.
    /// </summary>
    private Comment() { }

    /// <summary>
    /// Performs additional validation logic for the post entity.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when <see cref="Title"/> is null, empty, or exceeds 256 characters.</exception>
    /// <exception cref="ArgumentException">Thrown when <see cref="Content"/> is null or empty.</exception>
    public override void Validate()
    {
        base.Validate();
        ValidateAuthor(Author);
        ValidateContent(Content);
    }

    /// <summary>
    /// Validates the author of the comment.
    /// </summary>
    /// <param name="author">The author to validate.</param>
    private static void ValidateAuthor(string author)
    {
        if (string.IsNullOrWhiteSpace(author))
            throw new ArgumentException("Author must contain a value", nameof(author));

        if (author.Length > 256)
            throw new ArgumentException("Author max length is 256", nameof(author));
    }

    /// <summary>
    /// Validates the content of the comment.
    /// </summary>
    /// <param name="content">The content to validate.</param>
    private static void ValidateContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Content must contain a value", nameof(content));

        if (content.Length > 256)
            throw new ArgumentException("Content max length is 512", nameof(content));
    }
}
