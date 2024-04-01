using Blog.Domain;

namespace Blog.UnitTest.Domain.Entities;

public class CommentTests
{
    private Comment _comment;

    public CommentTests()
    {
        var postId = Guid.NewGuid();
        var id = Guid.NewGuid();
        var author = "Test Author";
        var content = "Test Content";
        _comment = new Comment(id, author, content, postId);
    }
    
    [Fact]
    public void Comment_Initialization_WithValidValues_Success()
    {
        var comment = new Comment(_comment.Id, _comment.Author, _comment.Content, _comment.PostId);

        Assert.Equal(_comment.Author, comment.Author);
        Assert.Equal(_comment.Content, comment.Content);
        Assert.Equal(_comment.PostId, comment.PostId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Comment_SetAuthor_NullOrEmpty_ThrowsArgumentException(string author)
    {
        Assert.Throws<ArgumentException>(() => _comment.Author = author);
    }

    [Fact]
    public void Comment_SetAuthor_ExceedsMaxLength_ThrowsArgumentException()
    {
        string author = new('X', Comment.AuthorMaxLength + 1);

        Assert.Throws<ArgumentException>(() => _comment.Author = author);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Comment_SetContent_NullOrEmpty_ThrowsArgumentException(string content)
    {
        Assert.Throws<ArgumentException>(() => _comment.Content = content);
    }

    [Fact]
    public void Comment_Initialization_ContentExceedsMaxLength_ThrowsArgumentException()
    {
        string content = new('X', Comment.ContentMaxLength + 1);
        Assert.Throws<ArgumentException>(() => _comment.Content = content);
    }
}