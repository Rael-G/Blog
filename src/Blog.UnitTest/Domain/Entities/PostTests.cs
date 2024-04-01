using Blog.Domain;

namespace Blog.UnitTest.Domain.Entities;

public class PostTests
{
    [Fact]
    public void Post_Initialization_WithValidValues_Success()
    {
        var id = Guid.NewGuid();
        var title = "My Post";
        var content = "Post content";

        var post = new Post(id, title, content);

        Assert.Equal(title, post.Title);
        Assert.Equal(content, post.Content);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Post_Initialization_TitleNullOrEmpty_ThrowsArgumentException(string title)
    {
        Guid id = Guid.NewGuid();
        string content = "Content";

        Assert.Throws<ArgumentException>(() => new Post(id, title, content));
    }

    [Fact]
    public void Post_Initialization_TitleExceedsMaxLength_ThrowsArgumentException()
    {
        Guid id = Guid.NewGuid();
        string title = new('X', Post.TitleMaxLength + 1);
        string content = "Content";

        Assert.Throws<ArgumentException>(() => new Post(id, title, content));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Post_Initialization_ContentNullOrEmpty_ThrowsArgumentException(string content)
    {
        Guid id = Guid.NewGuid();
        string title = "Title";

        Assert.Throws<ArgumentException>(() => new Post(id, title, content));
    }
}