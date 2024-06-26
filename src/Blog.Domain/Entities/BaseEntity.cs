﻿namespace Blog.Domain;
/// <summary>
/// Represents the base class for all entities in the Blog domain.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Gets the unique identifier of the entity.
    /// </summary>
    public Guid Id { get => _id; private set => _id = ValidateId(value); }

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

    private static DateTime ValidateTime(DateTime dateTime)
    {
        if (dateTime == DateTime.MinValue)
            throw new DomainException(nameof(dateTime));
        return dateTime;
    }

    private static Guid ValidateId(Guid id)
    {
        if (id == Guid.Empty)
            throw new DomainException(nameof(id));

        return id;
    }
}
