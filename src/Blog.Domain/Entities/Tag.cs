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
    /// <exception cref="DomainException">Thrown when name exceeds the maximum length.</exception>
    public string Name { get => _name; set => _name = ValidateName(value); }

    /// <summary>
    /// Gets the relation of posts associated with the tag.
    /// </summary>
    public IEnumerable<PostTag> Posts { get; }

    private string _name = "";

    /// <summary>
    /// Initializes a new instance of the tag class with the specified identifier and name.
    /// </summary>
    /// <param name="id">The identifier of the tag.</param>
    /// <param name="name">The name of the tag.</param>
    /// <exception cref="DomainException"></exception>
    public Tag(Guid id, string name) : base(id)
    {
        Name = name;
        Posts = new List<PostTag>();
    }

    private string ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException($"{nameof(name)} must contain a value");

        if (name.Length < NameMinLength)
            throw new DomainException($"{nameof(name)} minimum length is {NameMinLength}");

        if (name.Length > NameMaxLength)
            throw new DomainException($"{nameof(name)} maximum length is {NameMaxLength}");

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