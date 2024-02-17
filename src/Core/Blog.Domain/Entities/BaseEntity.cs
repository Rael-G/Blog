namespace Blog.Domain;

public abstract class BaseEntity(Guid? id)
{
    public Guid Id { get; private set; } = id??= Guid.NewGuid();
}
