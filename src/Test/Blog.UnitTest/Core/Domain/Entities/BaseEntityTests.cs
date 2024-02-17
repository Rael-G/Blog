using Blog.Domain;

namespace Blog.UnitTest.Core.Domain.Entities;

public class BaseEntityTests
{
    [Fact]
    public void BaseEntity_Id_Generated_When_Null_Id_Provided()
    {
        Guid? id = null;

        var entity = new ConcreteEntity(id);

        Assert.NotEqual(Guid.Empty, entity.Id);
    }

    [Fact]
    public void BaseEntity_Id_Set_When_Id_Provided()
    {
        Guid id = Guid.NewGuid();

        var entity = new ConcreteEntity(id);

        Assert.Equal(id, entity.Id);
    }
    
}

//Test purpose class
public class ConcreteEntity : BaseEntity
{
    public ConcreteEntity(Guid? id) : base(id) { }
}
