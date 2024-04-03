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
    public void Comment_SetAuthor_NullOrEmpty_ThrowsArgumentNullException(string author)
    {
        Assert.Throws<ArgumentNullException>(() => _comment.Author = author);
    }

    [Fact]
    public void Comment_SetAuthor_ExceedsMaxLength_ThrowsArgumentException()
    {
        var authorMaxlength = 100;
        string author = new('X', authorMaxlength + 1);

        Assert.Throws<ArgumentException>(() => _comment.Author = author);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Comment_SetContent_NullOrEmpty_ThrowsArgumentNullException(string content)
    {
        Assert.Throws<ArgumentNullException>(() => _comment.Content = content);
    }

    [Fact]
    public void Comment_Initialization_ContentExceedsMaxLength_ThrowsArgumentException()
    {
        var contentMaxLength = 255;
        string content = new('X', contentMaxLength + 1);
        Assert.Throws<ArgumentException>(() => _comment.Content = content);
    }
}