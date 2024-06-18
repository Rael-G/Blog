namespace Blog.Domain;

/// <summary>
/// Represents the many-to-many relationship between the Post and Tag entities.
/// </summary>
public class PostTag
{
    /// <summary>
    /// Gets the ID of the associated post.
    /// </summary>
    public Guid PostId { get => _postId; private set => _postId = ValidateId(value); }

    /// <summary>
    /// Gets the ID of the associated tag.
    /// </summary>
    public Guid TagId { get => _tagId; private set => _tagId = ValidateId(value); }

    /// <summary>
    /// Gets the associated Post object.
    /// </summary>
    public Post? Post { get; }

    /// <summary>
    /// Gets the associated Tag object.
    /// </summary>
    public Tag? Tag { get; }

    private Guid _postId = Guid.Empty;
    private Guid _tagId = Guid.Empty;

    /// <summary>
    /// Creates a new instance of the PostTag class with the specified IDs.
    /// </summary>
    /// <param name="postId">The ID of the post.</param>
    /// <param name="tagId">The ID of the tag.</param>
    /// <exception cref="DomainException"></exception>
    public PostTag(Guid postId, Guid tagId)
    {
        PostId = postId;
        TagId = tagId;
    }

    private static Guid ValidateId(Guid id)
    {
        if (id == Guid.Empty)
            throw new DomainException(nameof(id));

        return id;
    }
}