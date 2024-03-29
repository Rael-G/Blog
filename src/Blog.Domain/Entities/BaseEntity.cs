namespace Blog.Domain;

/// <summary>
/// Constructor for the BaseEntity class.
/// </summary>
/// <param name="id">The unique identifier of the entity, optional. If not provided, a new unique identifier will be generated.</param>
/// <param name="createdTime">The created time of the entity.</param>
/// <param name="updateTime">The update time of the entity.</param>
public abstract class BaseEntity(Guid id, DateTime createdTime, DateTime updateTime)
{
    /// <summary>
    /// Gets or sets the unique identifier of the entity.
    /// </summary>
    public Guid Id { get; private set; } = id;

    /// <summary>
    /// Gets or sets the created time of the entity.
    /// </summary>
    public DateTime CreatedTime { get; } = createdTime;

    /// <summary>
    /// Gets or sets the update time of the entity.
    /// </summary>
    public DateTime UpdateTime { get; set; } = updateTime;

    /// <summary>
    /// Virtual method to perform additional validation logic for the entity.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when <see cref="Id"/> is null or empty.</exception>
    /// <exception cref="ArgumentException">Thrown when <see cref="CreatedTime"/> is in the future.</exception>
    /// <exception cref="ArgumentException">Thrown when <see cref="UpdateTime"/> is in the future or is older than created time.</exception>
    public virtual void Validate()
    {
        ValidateId();
        ValidateCreatedTime();
        ValidateUpdateTime();
    }

    /// <summary>
    /// Validates the unique identifier of the entity.
    /// </summary>
    private void ValidateId()
    {
        if (Id == Guid.Empty) 
            throw new ArgumentNullException(nameof(Id));
    }

    /// <summary>
    /// Validates the created time of the entity.
    /// </summary>
    private void ValidateCreatedTime()
    {
        if (CreatedTime > DateTime.UtcNow)
            throw new ArgumentException("Created time can't be in the future", nameof(CreatedTime));
    }

    /// <summary>
    /// Validates the update time of the entity.
    /// </summary>
    private void ValidateUpdateTime()
    {
        if (UpdateTime > DateTime.UtcNow)
            throw new ArgumentException("Update time can't be in the future", nameof(UpdateTime));
        
        if (UpdateTime < CreatedTime)
            throw new ArgumentException("Update time can't be older than created time", nameof(UpdateTime));
    }
}
