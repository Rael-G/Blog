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

        Assert.Throws<ArgumentNullException>(() => new TestEntity(id));
    }

    [Fact]
    public void BaseEntity_Initialization_WithValidValues_Success()
    {
        var entity = new TestEntity(_id);

        Assert.Equal(_id, entity.Id);
    }

    [Fact]
    public void BaseEntity_Initialization_CreatedTimeIsUtcNow()
    {
        var before = DateTime.UtcNow.Subtract(TimeSpan.FromMicroseconds(1));
        var entity = new TestEntity(_id);
        var after = DateTime.UtcNow.AddMicroseconds(1);

        entity.CreatedTime.Should().BeAfter(before).And.BeBefore(after);
    }

    [Fact]
    public void BaseEntity_Initialization_ModifiedTimeIsCreatedTime()
    {
        var entity = new TestEntity(_id);

        Assert.Equal(entity.CreatedTime, entity.ModifiedTime);
    }
}