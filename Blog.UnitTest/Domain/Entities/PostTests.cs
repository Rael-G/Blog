using Blog.Domain;

namespace Blog.UnitTest.Domain.Entities;

public class PostTests
{
    Post _post;

    public PostTests()
    {
        var id = Guid.NewGuid();
        var title = "My Post";
        var content = "Post Content";
        var userId = Guid.NewGuid();

        _post = new(id, title, content, userId);

    }

    [Fact]
    public void Post_Initialization_WithValidValues_Success()
    {
        var post = new Post(_post.Id, _post.Title, _post.Content, _post.UserId);

        Assert.Equal(_post.Title, post.Title);
        Assert.Equal(_post.Content, post.Content);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Post_SetTitle_NullOrEmpty_ThrowsDomainException(string title)
    {
        Assert.Throws<DomainException>(() => _post.Title = title);
    }

    [Fact]
    public void Post_SetTitle_TitleExceedsMaxLength_ThrowsDomainException()
    {
        var titleMaxLength = 100;
        string title = new('X', titleMaxLength + 1);

        Assert.Throws<DomainException>(() => _post.Title = title);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Post_SetContent_NullOrEmpty_ThrowsDomainException(string content)
    {
        Assert.Throws<DomainException>(() => _post.Content = content);
    }
}