using Blog.Domain;

namespace Blog.UnitTest.Core.Domain.Entities;

public class PostTests
{
    [Fact]
    public void Post_Initialization_Without_GUID()
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
    public void Post_Initialization_With_GUID()
    {
        var guid = Guid.NewGuid();
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
        Assert.NotEqual(Guid.Empty, post.Id);
    }

    [Fact]
    public void Post_Initialization_With_Null_Comments()
    {
        string title = "Test Title";
        string content = "Test Content";

        var post = new Post(title, content, null);

        Assert.NotNull(post.Comments);
        Assert.Empty(post.Comments);
    }

    [Fact]
    public void Post_Editing_Title()
    {
        var post = new Post("Initial Title", "Initial Content", new List<Comment>());
        string newTitle = "New Title";

        post.Title = newTitle;

        Assert.Equal(newTitle, post.Title);
    }

    [Fact]
    public void Post_Editing_Content()
    {
        var post = new Post("Initial Title", "Initial Content", new List<Comment>());
        string newContent = "New Content";

        post.Content = newContent;

        Assert.Equal(newContent, post.Content);
    }
}