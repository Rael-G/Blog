using Blog.Domain;
using FluentAssertions;

namespace Blog.UnitTest.Domain.Entities;

public class BaseEntityTests
{
     private readonly Guid _validGuid;
    private readonly string _validString;
    private readonly string _parameterName;

    public BaseEntityTests()
    {
        _validGuid = Guid.NewGuid();
        _validString = "ValidString";
        _parameterName = "TestParameter";
    }

    [Fact]
    public void BaseEntity_Initialization_IdEmpty_ThrowsDomainException()
    {
        var id = Guid.Empty;
        var updated = DateTime.UtcNow;

        Assert.Throws<DomainException>(() => new TestEntity(id));
    }

    [Fact]
    public void BaseEntity_Initialization_WithValidValues_Success()
    {
        var entity = new TestEntity(_validGuid);

        Assert.Equal(_validGuid, entity.Id);
    }

    [Fact]
    public void BaseEntity_Initialization_CreatedTimeIsUtcNow()
    {
        var before = DateTime.UtcNow.Subtract(TimeSpan.FromMicroseconds(1));
        var entity = new TestEntity(_validGuid);
        var after = DateTime.UtcNow.AddMicroseconds(1);

        entity.CreatedTime.Should().BeAfter(before).And.BeBefore(after);
    }

    [Fact]
    public void BaseEntity_Initialization_ModifiedTimeIsCreatedTime()
    {
        var entity = new TestEntity(_validGuid);

        Assert.Equal(entity.CreatedTime, entity.ModifiedTime);
    }

    [Fact]
    public void ValidateStringNullOrWhiteSpace_ValidValue_ReturnsValue()
    {
        var result = TestEntity.TestValidateStringNullOrWhiteSpace(_validString, _parameterName);
        Assert.Equal(_validString, result);
    }

    [Fact]
    public void ValidateId_EmptyId_ThrowsDomainException()
    {
        var emptyId = Guid.Empty;
        var exception = Assert.Throws<DomainException>(() => 
            TestEntity.TestValidateId(emptyId, _parameterName));
        Assert.Equal($"{_parameterName} must not be empty.", exception.Message);
    }

    [Fact]
    public void ValidateId_ValidId_ReturnsId()
    {
        var result = TestEntity.TestValidateId(_validGuid, _parameterName);
        Assert.Equal(_validGuid, result);
    }
}