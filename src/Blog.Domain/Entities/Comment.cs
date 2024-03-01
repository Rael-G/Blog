using System.Diagnostics.CodeAnalysis;

namespace Blog.Domain;

/// <summary>
/// Represents a comment associated with a blog post.
/// </summary>
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
    /// Gets the associated post of the comment.
    /// </summary>
    public Post? Post { get; }

    /// <summary>
    /// Gets the identifier of the associated post.
    /// </summary>
    public Guid PostId { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Comment"/> class with the specified identifier, author, content, and associated post.
    /// </summary>
    /// <param name="id">The identifier of the comment.</param>
    /// <param name="author">The author of the comment.</param>
    /// <param name="content">The content of the comment.</param>
    /// <param name="postId">The Id of the associated post of the comment.</param>
    public Comment(Guid id, string author, string content, Guid postId) : base(id)
    {
        Author = author;
        Content = content;
        PostId = postId;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Comment"/> class with the specified identifier, author, content, and associated post.
    /// </summary>
    /// <param name="id">The identifier of the comment.</param>
    /// <param name="author">The author of the comment.</param>
    /// <param name="content">The content of the comment.</param>
    /// <param name="post">The associated post of the comment.</param>
    public Comment(Guid id, string author, string content, Post post) : base(id)
    {
        Author = author;
        Content = content;
        Post = post;
        PostId = post.Id;
    }

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

        if (content.Length > 512)
            throw new ArgumentException("Content max length is 512", nameof(content));
    }
}
