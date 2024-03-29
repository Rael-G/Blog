using Blog.Domain;
using Xunit.Sdk;

namespace Blog.UnitTest.Domain.Entities;

public class BaseEntityTests
{
    [Fact]
    public void BaseEntity_Validate_IdEmpty_ThrowsArgumentNullException()
    {
        var id = Guid.Empty;
        var created = DateTime.UtcNow;
        var updated = DateTime.UtcNow;
        var entity = new ConcreteEntity(id, created, updated);

        Assert.Throws<ArgumentNullException>(() => entity.Validate());
    }

    [Fact]
    public void BaseEntity_IdSet_WhenIdProvided()
    {
        Guid id = Guid.NewGuid();
        var created = DateTime.UtcNow;
        var updated = DateTime.UtcNow;
        var entity = new ConcreteEntity(id, created, updated);

        Assert.Equal(id, entity.Id);
    }

        [Fact]
    public void BaseEntity_Validate_CreatedTimeInFuture_ThrowsArgumentException()
    {
        Guid id = Guid.NewGuid();
        var created = DateTime.UtcNow.AddMinutes(5);
        var updated = DateTime.UtcNow;

        var entity = new ConcreteEntity(id, created, updated);

        Assert.Throws<ArgumentException>(() => entity.Validate());
    }

    [Fact]
    public void BaseEntity_Validate_UpdateTimeInFuture_ThrowsArgumentException()
    {
        Guid id = Guid.NewGuid();
        var created = DateTime.UtcNow;
        var updated = DateTime.UtcNow.AddMinutes(5);

        var entity = new ConcreteEntity(id, created, updated);

        Assert.Throws<ArgumentException>(() => entity.Validate());
    }

    [Fact]
    public void BaseEntity_Validate_UpdateTimeOlderThanCreatedTime_ThrowsArgumentException()
    {
        Guid id = Guid.NewGuid();
        var created = DateTime.UtcNow;
        var updated = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(5));

        var entity = new ConcreteEntity(id, created, updated);

        Assert.Throws<ArgumentException>(() => entity.Validate());
    }

}

//Test purpose class
public class ConcreteEntity : BaseEntity
{
    public ConcreteEntity(Guid id, DateTime createdDate, DateTime updateTime) : base(id, createdDate, updateTime) { }
}
