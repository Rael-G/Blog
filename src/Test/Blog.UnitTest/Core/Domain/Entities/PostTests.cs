using Blog.Domain;

namespace Blog.UnitTest.Core.Domain.Entities;

public class PostTests
{
    [Fact]
    public void Post_Initialization_WithoutGUID()
    {
        var title = "My Post";
        var content = "Post content";
        var author = "John Doe";
        var commentContent = "Test comment";
        var comment = new Comment(author, commentContent);
        var comments = new List<Comment> { comment };

        var post = new Post(title, content, comments);

        Assert.Equal(title, post.Title);
        Assert.Equal(content, post.Content);
        Assert.Equal(comments, post.Comments);
    }

    [Fact]
    public void Post_Initialization_WithGUID()
    {
        var guid = Guid.NewGuid();
        var title = "My Post";
        var content = "Post content";
        var author = "John Doe";
        var commentContent = "Test comment";
        var comment = new Comment(author, commentContent);
        var comments = new List<Comment> { comment };

        var post = new Post(guid, title, content, comments);

        Assert.Equal(guid, post.Id);
        Assert.Equal(title, post.Title);
        Assert.Equal(content, post.Content);
        Assert.Equal(comments, post.Comments);
    }

    [Fact]
    public void Post_Initialization_WithNullComments()
    {
        string title = "Test Title";
        string content = "Test Content";

        var post = new Post(title, content, null);

        Assert.NotNull(post.Comments);
        Assert.Empty(post.Comments);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Post_TitleNullOrEmpty_ThrowsArgumentException(string title)
    {
        string content = "Content";
        IEnumerable<Comment> comments = [];

        Assert.Throws<ArgumentException>(() => new Post(title, content, comments));
    }

    [Fact]
    public void Post_TitleExceedsMaxLength_ThrowsArgumentException()
    {
        string title = new string('X', 257);
        string content = "Content";
        IEnumerable<Comment> comments = [];

        Assert.Throws<ArgumentException>(() => new Post(title, content, comments));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Post_ContentNullOrEmpty_ThrowsArgumentException(string content)
    {
        string title = "Title";
        IEnumerable<Comment> comments = [];

        Assert.Throws<ArgumentException>(() => new Post(title, content, comments));
    }
}