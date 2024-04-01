using Blog.Domain;

namespace Blog.UnitTest.Domain.Entities;

public class CommentTests
{
    private readonly Guid _postId = Guid.NewGuid();
    [Fact]
    public void Comment_Initialization_WithValidValues_Success()
    {
        Guid id = Guid.NewGuid();
        string author = "Test Author";
        string content = "Test Content";

        var comment = new Comment(id, author, content, _postId);

        Assert.Equal(author, comment.Author);
        Assert.Equal(content, comment.Content);
        Assert.Equal(_postId, comment.PostId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Comment_Initialization_AuthorNullOrEmpty_ThrowsArgumentException(string author)
    {
        Guid id = Guid.NewGuid();
        string content = "Content";

        Assert.Throws<ArgumentException>(() => new Comment(id, author, content, _postId));
    }

    [Fact]
    public void Comment_Initialization_AuthorExceedsMaxLength_ThrowsArgumentException()
    {
        Guid id = Guid.NewGuid();
        string author = new('X', Comment.AuthorMaxLength + 1);
        string content = "Content";

        Assert.Throws<ArgumentException>(() => new Comment(id, author, content, _postId));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Comment_Initialization_ContentNullOrEmpty_ThrowsArgumentException(string content)
    {
        Guid id = Guid.NewGuid();
        string author = "Author";

        Assert.Throws<ArgumentException>(() => new Comment(id, author, content, _postId));
    }

    [Fact]
    public void Comment_Initialization_ContentExceedsMaxLength_ThrowsArgumentException()
    {
        var contentMaxLength = 255;
        Guid id = Guid.NewGuid();
        string author = "Author";
        string content = new('X', Comment.ContentMaxLength + 1);
        Assert.Throws<ArgumentException>(() => new Comment(id, author, content, _postId));
    }
}
