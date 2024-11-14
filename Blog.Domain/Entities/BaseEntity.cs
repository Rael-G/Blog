namespace Blog.Domain;
/// <summary>
/// Represents the base class for all entities in the Blog domain.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Gets the unique identifier of the entity.
    /// </summary>
    public Guid Id { get => _id; private set => _id = ValidateId(value, nameof(Id)); }

    /// <summary>
    /// Gets or sets the created time of the entity.
    /// </summary>
    /// <exception cref="DomainException"></exception>
    public DateTime CreatedTime { get => _createdTime; set => _createdTime = ValidateTime(value); }

    /// <summary>
    /// Gets the modified time of the entity.
    /// </summary>
    /// <exception cref="DomainException"></exception>
    public DateTime ModifiedTime { get => _modifiedTime; set => _modifiedTime = ValidateTime(value); }

    private Guid _id = Guid.Empty;
    private DateTime _createdTime;
    private DateTime _modifiedTime;

    /// <summary>
    /// Constructor for the BaseEntity class.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <exception cref="DomainException"></exception>
    public BaseEntity(Guid id)
    {
        var now = DateTime.UtcNow;

        Id = id;
        CreatedTime = now;
        ModifiedTime = now;
    }

    /// <summary>
    /// Validates that a string is neither null nor consists only of white-space characters.
    /// </summary>
    /// <param name="value">The string value to validate.</param>
    /// <param name="parameterName">The name of the parameter being validated, used in the exception message.</param>
    /// <returns>The validated string value if it is not null or white-space.</returns>
    /// <exception cref="DomainException">Thrown when the string is null or consists only of white-space characters.</exception>
    protected static string ValidateStringNullOrWhiteSpace(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException($"{parameterName} must contain a value.");

        return value;
    }

    /// <summary>
    /// Validates that a GUID is not empty.
    /// </summary>
    /// <param name="id">The GUID value to validate.</param>
    /// <param name="parameterName">The name of the parameter being validated, used in the exception message.</param>
    /// <returns>The validated GUID if it is not empty.</returns>
    /// <exception cref="DomainException">Thrown when the GUID is empty.</exception>
    protected static Guid ValidateId(Guid id, string parameterName)
    {
        if (id == Guid.Empty)
            throw new DomainException($"{parameterName} must not be empty.");

        return id;
    }

    private static DateTime ValidateTime(DateTime dateTime)
    {
        if (dateTime == DateTime.MinValue)
            throw new DomainException(nameof(dateTime));
        return dateTime;
    }

    
}
