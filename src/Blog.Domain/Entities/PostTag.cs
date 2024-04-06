namespace Blog.Domain;

/// <summary>
/// Represents the many-to-many relationship between the Post and Tag entities.
/// </summary>
public class PostTag
{
    /// <summary>
    /// Gets or sets the ID of the associated post.
    /// </summary>
    public Guid PostId { get; }

    /// <summary>
    /// Gets or sets the ID of the associated tag.
    /// </summary>
    public Guid TagId { get; }

    /// <summary>
    /// Gets or sets the associated Post object.
    /// </summary>
    public Post? Post { get; }

    /// <summary>
    /// Gets or sets the associated Tag object.
    /// </summary>
    public Tag? Tag { get; }

    /// <summary>
    /// Creates a new instance of the PostTag class with the specified IDs.
    /// </summary>
    /// <param name="postId">The ID of the post.</param>
    /// <param name="tagId">The ID of the tag.</param>
    public PostTag(Guid postId, Guid tagId)
    {
        PostId = postId;
        TagId = tagId;
    }
}