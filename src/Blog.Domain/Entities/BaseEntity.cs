namespace Blog.Domain;

/// <summary>
/// Constructor for the BaseEntity class.
/// </summary>
/// <param name="id">The unique identifier of the entity, optional. If not provided, a new unique identifier will be generated.</param>
public abstract class BaseEntity(Guid id)
{
    /// <summary>
    /// Gets or sets the unique identifier of the entity.
    /// </summary>
    public Guid Id { get; private set; } = id;

    /// <summary>
    /// Virtual method to perform additional validation logic for the entity.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when <see cref="Id"/> is null or empty.</exception>
    public virtual void Validate()
    {
        ValidateId(Id);
    }

    /// <summary>
    /// Private method to validate the unique identifier of the entity.
    /// </summary>
    /// <param name="id">The identifier to be validated.</param>
    /// <returns>The set identifier or a new unique identifier if the provided identifier is null or empty.</returns>
    private static void ValidateId(Guid? id)
    {
        if (id == null || id == Guid.Empty) 
            throw new ArgumentNullException(nameof(id));
    }
}
