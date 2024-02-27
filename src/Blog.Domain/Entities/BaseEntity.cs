namespace Blog.Domain;

public abstract class BaseEntity
{
    /// <summary>
    /// Gets or sets the unique identifier of the entity.
    /// </summary>
    public Guid Id { get => _id; private set => _id = SetId(value); }

    private Guid _id;

    /// <summary>
    /// Constructor for the BaseEntity class.
    /// </summary>
    /// <param name="id">The unique identifier of the entity, optional. If not provided, a new unique identifier will be generated.</param>
    protected BaseEntity(Guid? id = null)
    {
        Id = id ?? Guid.NewGuid();
    }

    /// <summary>
    /// Private method to validate the unique identifier of the entity.
    /// </summary>
    /// <param name="id">The identifier to be validated.</param>
    /// <returns>The valid identifier or a new unique identifier if the provided identifier is null or empty.</returns>
    private static Guid SetId(Guid? id)
    {
        if (id == Guid.Empty) 
            id = Guid.NewGuid();

        return id ?? Guid.NewGuid();
    }
}
