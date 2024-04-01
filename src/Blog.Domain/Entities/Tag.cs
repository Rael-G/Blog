namespace Blog.Domain;

/// <summary>
/// Represents a tag for a blog post.
/// </summary>
public class Tag : BaseEntity
{
    /// <summary>
    /// Minimum length allowed for the name of the tag.
    /// </summary>
    public const int NameMinLength = 3;

    /// <summary>
    /// Maximum length allowed for the name of the tag.
    /// </summary>
    public const int NameMaxLength = 15;

    /// <summary>
    /// Gets or sets the name of the tag.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when <see cref="Name"/> is null, empty, or exceeds the maximum length.</exception>
    public string Name { get => _name; set => _name = ValidateName(value); }

    /// <summary>
    /// Gets the blog posts associated with the tag.
    /// </summary>
    public IEnumerable<Post> Posts { get; }

    private string _name = "";

    /// <summary>
    /// Initializes a new instance of the <see cref="Tag"/> class with the specified identifier and name.
    /// </summary>
    /// <param name="id">The identifier of the tag.</param>
    /// <param name="name">The name of the tag.</param>
    public Tag(Guid id, string name) : base(id)
    {
        Name = name;
        Posts = new List<Post>();
    }

    private string ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException($"{nameof(name)} must contain a value", nameof(name));

        if (name.Length < NameMinLength)
            throw new ArgumentException($"{nameof(name)} minimum length is {NameMinLength}", nameof(name));

        if (name.Length > NameMaxLength)
            throw new ArgumentException($"{nameof(name)} maximum length is {NameMaxLength}", nameof(name));
        
        return name;
    }

    /// <summary>
    /// Returns the string representation of the tag.
    /// </summary>
    public override string ToString()
    {
        return Name;
    }
}

