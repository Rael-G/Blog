namespace Blog.Domain.Entities;

public abstract class BaseEntity(Guid? id)
{
    public Guid Id { get; set; } = id??= Guid.NewGuid();
}
