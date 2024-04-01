using System.Configuration;
using Blog.Domain;
using FluentAssertions;

namespace Blog.UnitTest.Domain.Entities;

public class BaseEntityTests
{
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
        Guid id = Guid.NewGuid();
        var entity = new ConcreteEntity(id);

        Assert.Equal(id, entity.Id);
    }

    [Fact]
    public void BaseEntity_Initialization_CreatedTimeIsUtcNow()
    {
        Guid id = Guid.NewGuid();
        var before = DateTime.UtcNow.Subtract(TimeSpan.FromMicroseconds(1));
        var entity = new ConcreteEntity(id);
        var after = DateTime.UtcNow.AddMicroseconds(1);

        entity.CreatedTime.Should().BeAfter(before).And.BeBefore(after);
    }

    [Fact]
    public void BaseEntity_Initialization_ModifiedTimeIsCreatedTime()
    {
        Guid id = Guid.NewGuid();

        var entity = new ConcreteEntity(id);

        Assert.Equal(entity.CreatedTime, entity.ModifiedTime);
    }

    [Fact]
    public void BaseEntity_UpdateTime_ModifiedTimeIsUtcNow()
    {
        Guid id = Guid.NewGuid();
        var entity = new ConcreteEntity(id);

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
