using Blog.Domain;

namespace Blog.UnitTest.Domain.Entities;

public class CommentTests
{
    private readonly Post _post = new(Guid.NewGuid(), DateTime.Now.Subtract(TimeSpan.FromMinutes(5)), DateTime.Now, "Title", "Content", []);

    [Fact]
    public void Comment_Initialization()
    {
        Guid id = Guid.NewGuid();
        string author = "Test Author";
        string content = "Test Content";
        var created = DateTime.Now;
        var updated = DateTime.Now;

        var comment = new Comment(id, created, updated, author, content, _post);

        Assert.Equal(id, comment.Id);
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
        var created = DateTime.Now;
        var updated = DateTime.Now;

        var comment = new Comment(id, created, updated, author, content, _post);

        Assert.Throws<ArgumentException>(() => comment.Validate());
    }

    [Fact]
    public void Comment_Validate_AuthorExceedsMaxLength_ThrowsArgumentException()
    {
        Guid id = Guid.NewGuid();
        string author = new('X', 257);
        string content = "Content";
        var created = DateTime.Now;
        var updated = DateTime.Now;

        var comment = new Comment(id, created, updated, author, content, _post);

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
        var created = DateTime.Now;
        var updated = DateTime.Now;

        var comment = new Comment(id, created, updated, author, content, _post);

        Assert.Throws<ArgumentException>(() => comment.Validate());
    }

    [Fact]
    public void Comment_Validate_ContentExceedsMaxLength_ThrowsArgumentException()
    {
        Guid id = Guid.NewGuid();
        string author = "Author";
        string content = new('X', 513);
        var created = DateTime.Now;
        var updated = DateTime.Now;

        var comment = new Comment(id, created, updated, author, content, _post);

        Assert.Throws<ArgumentException>(() => comment.Validate());
    }
}
