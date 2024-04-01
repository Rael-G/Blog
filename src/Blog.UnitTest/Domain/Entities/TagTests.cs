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
    public void Tag_SetName_NullOrEmpty_ThrowsArgumentException(string name)
    {
        Assert.Throws<ArgumentException>(() => _tag.Name = name);
    }

    [Fact]
    public void Comment_SetName_NameLessThanMinLength_ThrowsArgumentException()
    {
        string name = new string('X', Tag.NameMinLength - 1);

        Assert.Throws<ArgumentException>(() => _tag.Name = name);
    }

    [Fact]
    public void Tag_SetName_NameExceedsMaxLength_ThrowsArgumentException()
    {
        Guid id = Guid.NewGuid();
        string name = new('X', Tag.NameMaxLength + 1);

        Assert.Throws<ArgumentException>(() => _tag.Name = name);
    }
}