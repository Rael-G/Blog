namespace Blog.Domain;

public abstract class BaseEntity
{
    public Guid Id { get => _id; private set => _id = SetId(value); }

    private Guid _id;

    protected BaseEntity(Guid? id = null)
    {
        Id = id ?? Guid.NewGuid();
    }

    private Guid SetId(Guid? id)
    {
        if (id == Guid.Empty) 
            id = Guid.NewGuid();

        return id ?? Guid.NewGuid();
    }
}
