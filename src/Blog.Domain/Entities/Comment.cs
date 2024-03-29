namespace Blog.Domain;

public class Comment : BaseEntity
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
    /// Initializes a new instance of the <see cref="Comment"/> class with the specified identifier, author, content, created time, update time and associated post.
    /// </summary>
    /// <param name="id">The identifier of the comment.</param>
    /// <param name="author">The author of the comment.</param>
    /// <param name="content">The content of the comment.</param>
    /// <param name="createdTime">The created time of the comment</param>
    /// <param name="updateTime">The update time of the comment</param>
    /// <param name="postId">The Id of the associated post of the comment.</param>
    
    public Comment(Guid id, DateTime createdTime, DateTime updateTime, string author, string content, Guid postId) : base(id, createdTime, updateTime)
    {
        Author = author;
        Content = content;
        PostId = postId;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Comment"/> class with the specified identifier, author, content, created time, update time and associated post.
    /// </summary>
    /// <param name="id">The identifier of the comment.</param>
    /// <param name="createdTime">The created time of the comment</param>
    /// <param name="updateTime">The update time of the comment</param>
    /// <param name="author">The author of the comment.</param>
    /// <param name="content">The content of the comment.</param>
    /// <param name="post">The associated post of the comment.</param>
    
    public Comment(Guid id, DateTime createdTime, DateTime updateTime, string author, string content, Post post) : base(id, createdTime, updateTime)
    {
        Author = author;
        Content = content;
        Post = post;
        PostId = post.Id;
    }

    /// <summary>
    /// Performs additional validation logic for the post entity.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when <see cref="Author"/> is null, empty, or exceeds 256 characters.</exception>
    /// <exception cref="ArgumentException">Thrown when <see cref="Content"/> is null or empty.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <see cref="Id"/> is null or empty.</exception>
    /// <exception cref="ArgumentException">Thrown when <see cref="CreatedTime"/> is in the future.</exception>
    /// <exception cref="ArgumentException">Thrown when <see cref="UpdateTime"/> is in the future or is older than created time.</exception>
    public override void Validate()
    {
        base.Validate();
        ValidateAuthor();
        ValidateContent();
    }

    /// <summary>
    /// Validates the author of the comment.
    /// </summary>
    /// <param name="author">The author to validate.</param>
    private void ValidateAuthor()
    {
        if (string.IsNullOrWhiteSpace(Author))
            throw new ArgumentException("Author must contain a value", nameof(Author));

        if (Author.Length > 256)
            throw new ArgumentException("Author max length is 256", nameof(Author));
    }

    /// <summary>
    /// Validates the content of the comment.
    /// </summary>
    /// <param name="content">The content to validate.</param>
    private void ValidateContent()
    {
        if (string.IsNullOrWhiteSpace(Content))
            throw new ArgumentException("Content must contain a value", nameof(Content));

        if (Content.Length > 512)
            throw new ArgumentException("Content max length is 512", nameof(Content));
    }
}