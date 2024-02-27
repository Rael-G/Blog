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
    public IEnumerable<Comment> Comments { get; private set; }

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

    /// <summary>
    /// Private constructor for entity framework use.
    /// </summary>
    private Post() { }

    /// <summary>
    /// Performs additional validation logic for the post entity.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when <see cref="Title"/> is null, empty, or exceeds 256 characters.</exception>
    /// <exception cref="ArgumentException">Thrown when <see cref="Content"/> is null or empty.</exception>
    public override void Validate()
    {
        base.Validate();
        ValidateTitle(Title);
        ValidateContent(Content);
    }

    /// <summary>
    /// Validates the title of the post.
    /// </summary>
    /// <param name="title">The title to validate.</param>
    private static void ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title must contain a value", nameof(title));

        if (title.Length > 256)
            throw new ArgumentException("Title max length is 256", nameof(title));
    }

    /// <summary>
    /// Validates the content of the post.
    /// </summary>
    /// <param name="content">The content to validate.</param>
    private static void ValidateContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Content must contain a value", nameof(content));
    }
}