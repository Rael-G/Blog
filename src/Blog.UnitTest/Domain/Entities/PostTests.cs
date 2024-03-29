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
        var created = DateTime.Now;
        var updated = DateTime.Now;

        var post = new Post(id, created, updated, title, content);

        Assert.Equal(id, post.Id);
        Assert.Equal(title, post.Title);
        Assert.Equal(content, post.Content);
        Assert.Equal(created, post.CreatedTime);
        Assert.Equal(updated, post.UpdateTime);

    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Post_Validate_TitleNullOrEmpty_ThrowsArgumentException(string title)
    {
        Guid id = Guid.NewGuid();
        string content = "Content";
        var created = DateTime.Now;
        var updated = DateTime.Now;

        var post = new Post(id, created, updated, title, content);

        Assert.Throws<ArgumentException>(() => post.Validate());
    }

    [Fact]
    public void Post_Validate_TitleExceedsMaxLength_ThrowsArgumentException()
    {
        Guid id = Guid.NewGuid();
        string title = new string('X', 257);
        string content = "Content";
        var created = DateTime.Now;
        var updated = DateTime.Now;

        var post = new Post(id, created, updated, title, content);

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
        var created = DateTime.Now;
        var updated = DateTime.Now;

        var post = new Post(id, created, updated, title, content);

        Assert.Throws<ArgumentException>(() => post.Validate());
    }
}