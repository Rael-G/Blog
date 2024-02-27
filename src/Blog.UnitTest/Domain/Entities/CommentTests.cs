using Blog.Domain;
using System;

namespace Blog.UnitTest.Domain.Entities;

public class CommentTests
{
    [Fact]
    public void Comment_Initialization_WithGuid()
    {
        Guid guid = Guid.NewGuid();
        string author = "Test Author";
        string content = "Test Content";

        var comment = new Comment(guid, author, content);

        Assert.Equal(guid, comment.Id);
        Assert.Equal(author, comment.Author);
        Assert.Equal(content, comment.Content);
    }

    [Fact]
    public void Comment_Initialization_WithoutGuid()
    {

        string author = "Test Author";
        string content = "Test Content";

        var comment = new Comment(author, content);

        Assert.Equal(author, comment.Author);
        Assert.Equal(content, comment.Content);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Comment_Validate_AuthorNullOrEmpty_ThrowsArgumentException(string author)
    {
        string content = "Content";
        var comment = new Comment(author, content);

        Assert.Throws<ArgumentException>(() => comment.Validate());
    }

    [Fact]
    public void Comment_Validate_AuthorExceedsMaxLength_ThrowsArgumentException()
    {
        string author = new('X', 257);
        string content = "Content";
        var comment = new Comment(author, content);

        Assert.Throws<ArgumentException>(() => comment.Validate());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Comment_Validate_ContentNullOrEmpty_ThrowsArgumentException(string content)
    {
        string author = "Author";
        var comment = new Comment(author, content);

        Assert.Throws<ArgumentException>(() => comment.Validate());
    }

    [Fact]
    public void Comment_Validate_ContentExceedsMaxLength_ThrowsArgumentException()
    {
        string author = "Author";
        string content = new('X', 513);
        var comment = new Comment(author, content);

        Assert.Throws<ArgumentException>(() => comment.Validate());
    }
}
