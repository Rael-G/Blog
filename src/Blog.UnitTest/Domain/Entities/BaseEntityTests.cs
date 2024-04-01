using System.Configuration;
using Blog.Domain;
using FluentAssertions;

namespace Blog.UnitTest.Domain.Entities;

public class BaseEntityTests
{
    Guid _id;

    public BaseEntityTests()
    {
        _id = Guid.NewGuid();
    }

    [Fact]
    public void BaseEntity_Initialization_IdEmpty_ThrowsArgumentNullException()
    {
        var id = Guid.Empty;
        var updated = DateTime.UtcNow;

        Assert.Throws<ArgumentNullException>(() => new ConcreteEntity(id));
    }

    [Fact]
    public void BaseEntity_Initialization_WithValidValues_Success()
    {
        var entity = new ConcreteEntity(_id);

        Assert.Equal(_id, entity.Id);
    }

    [Fact]
    public void BaseEntity_Initialization_CreatedTimeIsUtcNow()
    {
        var before = DateTime.UtcNow.Subtract(TimeSpan.FromMicroseconds(1));
        var entity = new ConcreteEntity(_id);
        var after = DateTime.UtcNow.AddMicroseconds(1);

        entity.CreatedTime.Should().BeAfter(before).And.BeBefore(after);
    }

    [Fact]
    public void BaseEntity_Initialization_ModifiedTimeIsCreatedTime()
    {
        var entity = new ConcreteEntity(_id);

        Assert.Equal(entity.CreatedTime, entity.ModifiedTime);
    }

    [Fact]
    public void BaseEntity_UpdateTime_ModifiedTimeIsUtcNow()
    {
        var entity = new ConcreteEntity(_id);

        var before = DateTime.UtcNow.Subtract(TimeSpan.FromMicroseconds(1));
        entity.UpdateTime();
        var after = DateTime.UtcNow.AddMicroseconds(1);

        entity.ModifiedTime.Should().BeAfter(before).And.BeBefore(after);
    }

}

//Test purpose class
public class ConcreteEntity : BaseEntity
{
    public ConcreteEntity(Guid id) : base(id) { }
}
