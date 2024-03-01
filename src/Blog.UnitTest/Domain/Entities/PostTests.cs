using Blog.Domain;

namespace Blog.UnitTest.Domain.Entities;

public class PostTests
{
    [Fact]
    public void Post_Initialization()
    {
        var id = Guid.NewGuid();
        var title = "My Post";
        var content = "Post content";

        var post = new Post(id, title, content, []);

        Assert.Equal(id, post.Id);
        Assert.Equal(title, post.Title);
        Assert.Equal(content, post.Content);
    }

    [Fact]
    public void Post_Initialization_WithNullComments()
    {
        Guid id = Guid.NewGuid();
        string title = "Test Title";
        string content = "Test Content";

        var post = new Post(id, title, content, null);

        Assert.NotNull(post.Comments);
        Assert.Empty(post.Comments);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Post_Validate_TitleNullOrEmpty_ThrowsArgumentException(string title)
    {
        Guid id = Guid.NewGuid();
        string content = "Content";
        IEnumerable<Comment> comments = [];

        var post = new Post(id, title, content, comments);

        Assert.Throws<ArgumentException>(() => post.Validate());
    }

    [Fact]
    public void Post_Validate_TitleExceedsMaxLength_ThrowsArgumentException()
    {
        Guid id = Guid.NewGuid();
        string title = new string('X', 257);
        string content = "Content";
        IEnumerable<Comment> comments = [];

        var post = new Post(id, title, content, comments);

        Assert.Throws<ArgumentException>(() => post.Validate());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Post_Validate_ContentNullOrEmpty_ThrowsArgumentException(string content)
    {
        Guid id = Guid.NewGuid();
        string title = "Title";
        IEnumerable<Comment> comments = [];

        var post = new Post(id, title, content, comments);

        Assert.Throws<ArgumentException>(() => post.Validate());
    }
}