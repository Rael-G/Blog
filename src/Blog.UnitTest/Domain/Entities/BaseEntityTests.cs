using Blog.Domain;
using Xunit.Sdk;

namespace Blog.UnitTest.Domain.Entities;

public class BaseEntityTests
{
    [Fact]
    public void BaseEntity_Validate_IdGenerated_WhenEmptyGuidProvided()
    {
        var id = Guid.Empty;
        var entity = new ConcreteEntity(id);

        Assert.Throws<ArgumentNullException>(() => entity.Validate());
    }

    [Fact]
    public void BaseEntity_IdSet_WhenIdProvided()
    {
        Guid id = Guid.NewGuid();

        var entity = new ConcreteEntity(id);

        Assert.Equal(id, entity.Id);
    }

}

//Test purpose class
public class ConcreteEntity : BaseEntity
{
    public ConcreteEntity(Guid id) : base(id) { }
}
