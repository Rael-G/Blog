namespace Blog.Domain;

public class Post : BaseEntity
{
    /// <summary>
    /// Gets or sets the title of the post.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the content of the post.
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// Gets the comments associated with the post.
    /// </summary>
    public IEnumerable<Comment> Comments { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Post"/> class with the specified identifier, title, content, created time, update time, and comments.
    /// </summary>
    /// <param name="id">The identifier of the post.</param>
    /// <param name="createdTime">The created time of the post.</param>
    /// <param name="updateTime">The update time of the post.</param>
    /// <param name="title">The title of the post.</param>
    /// <param name="content">The content of the post.</param>
    /// <param name="createdTime">The created time of the post.</param>
    /// <param name="updateTime">The update time of the post.</param>
    /// <param name="comments">Optional comments associated with the post.</param>
    public Post(Guid id, DateTime createdTime, DateTime updateTime, string title, string content, IEnumerable<Comment> comments)
        : base(id, createdTime, updateTime)
    {
        Title = title;
        Content = content;
        Comments = comments;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Post"/> class with the specified identifier, title, content, created time, and update time.
    /// </summary>
    /// <param name="id">The identifier of the post.</param>
    /// <param name="createdTime">The created time of the post.</param>
    /// <param name="updateTime">The update time of the post.</param>
    /// <param name="title">The title of the post.</param>
    /// <param name="content">The content of the post.</param>
    public Post(Guid id, DateTime createdTime, DateTime updateTime, string title, string content)
        : base(id, createdTime, updateTime)
    {
        Title = title;
        Content = content;
        Comments = new List<Comment>();
    }

    /// <summary>
    /// Performs additional validation logic for the post entity.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when <see cref="Title"/> is null, empty, or exceeds 256 characters.</exception>
    /// <exception cref="ArgumentException">Thrown when <see cref="Content"/> is null or empty.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <see cref="Id"/> is empty.</exception>
    /// <exception cref="ArgumentException">Thrown when <see cref="CreatedTime"/> is in the future.</exception>
    /// <exception cref="ArgumentException">Thrown when <see cref="UpdateTime"/> is in the future or is older than created time.</exception>
    public override void Validate()
    {
        base.Validate();
        ValidateTitle();
        ValidateContent();
    }

    /// <summary>
    /// Validates the title of the post.
    /// </summary>
    private void ValidateTitle()
    {
        if (string.IsNullOrWhiteSpace(Title))
            throw new ArgumentException("Title must contain a value", nameof(Title));

        if (Title.Length > 256)
            throw new ArgumentException("Title max length is 256", nameof(Title));
    }

    /// <summary>
    /// Validates the content of the post.
    /// </summary>
    private void ValidateContent()
    {
        if (string.IsNullOrWhiteSpace(Content))
            throw new ArgumentException("Content must contain a value", nameof(Content));
    }
}