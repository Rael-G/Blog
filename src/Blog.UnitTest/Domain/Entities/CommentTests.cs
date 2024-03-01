using Blog.Domain;
using System;

namespace Blog.UnitTest.Domain.Entities;

public class CommentTests
{
    Post _post = new(Guid.NewGuid(), "Title", "Content", []);

    [Fact]
    public void Comment_Initialization()
    {
        Guid guid = Guid.NewGuid();
        string author = "Test Author";
        string content = "Test Content";

        var comment = new Comment(guid, author, content, _post);

        Assert.Equal(guid, comment.Id);
        Assert.Equal(author, comment.Author);
        Assert.Equal(content, comment.Content);
        Assert.Equal(_post, comment.Post);
        Assert.Equal(_post.Id, comment.PostId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Comment_Validate_AuthorNullOrEmpty_ThrowsArgumentException(string author)
    {
        Guid id = Guid.NewGuid();
        string content = "Content";
        var comment = new Comment(id, author, content, _post);

        Assert.Throws<ArgumentException>(() => comment.Validate());
    }

    [Fact]
    public void Comment_Validate_AuthorExceedsMaxLength_ThrowsArgumentException()
    {
        Guid id = Guid.NewGuid();
        string author = new('X', 257);
        string content = "Content";
        var comment = new Comment(id, author, content, _post);

        Assert.Throws<ArgumentException>(() => comment.Validate());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Comment_Validate_ContentNullOrEmpty_ThrowsArgumentException(string content)
    {
        Guid id = Guid.NewGuid();
        string author = "Author";
        var comment = new Comment(id, author, content, _post);

        Assert.Throws<ArgumentException>(() => comment.Validate());
    }

    [Fact]
    public void Comment_Validate_ContentExceedsMaxLength_ThrowsArgumentException()
    {
        Guid id = Guid.NewGuid();
        string author = "Author";
        string content = new('X', 513);
        var comment = new Comment(id, author, content, _post);

        Assert.Throws<ArgumentException>(() => comment.Validate());
    }
}
