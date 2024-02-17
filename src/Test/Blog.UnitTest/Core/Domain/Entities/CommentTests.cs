using Blog.Domain;

namespace Blog.UnitTest.Core.Domain.Entities;

public class CommentTests
{

    [Fact]
    public void Comment_Initialization()
    {
        string author = "Test Author";
        string content = "Test Content";

        var comment = new Comment(author, content);

        Assert.Equal(author, comment.Author);
        Assert.Equal(content, comment.Content);
    }

    [Fact]
    public void Comment_Editing_Author()
    {
        var comment = new Comment("Initial Author", "Test Content");
        string newAuthor = "New Author";

        comment.Author = newAuthor;

        Assert.Equal(newAuthor, comment.Author);
    }

    [Fact]
    public void Comment_Editing_Content()
    {
        var comment = new Comment("Test Author", "Initial Content");
        string newContent = "New Content";

        comment.Content = newContent;

        Assert.Equal(newContent, comment.Content);
    }
}
