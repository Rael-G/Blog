using Blog.Domain;

namespace Blog.UnitTest.Domain.Entities;

public class TagTests
{
    private Tag _tag;

    public TagTests()
    {
        var id = Guid.NewGuid();
        var name = "Test Tag";
        _tag = new(id, name);
    }

    [Fact]
    public void Tag_Initialization_WithValidValues_Success()
    {
        var tag = new Tag(_tag.Id, _tag.Name);

        Assert.Equal(_tag.Name, tag.Name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Tag_SetName_NullOrEmpty_ThrowsDomainException(string name)
    {
        Assert.Throws<DomainException>(() => _tag.Name = name);
    }

    [Fact]
    public void Comment_SetName_NameLessThanMinLength_ThrowsDomainException()
    {
        var nameMinLength = 3;
        string name = new string('X', nameMinLength - 1);

        Assert.Throws<DomainException>(() => _tag.Name = name);
    }

    [Fact]
    public void Tag_SetName_NameExceedsMaxLength_ThrowsDomainException()
    {
        var nameMaxLength = 15;
        Guid id = Guid.NewGuid();
        string name = new('X', nameMaxLength + 1);

        Assert.Throws<DomainException>(() => _tag.Name = name);
    }
}