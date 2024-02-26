namespace Blog.Domain;

public class Comment
    : BaseEntity
{
    public string Author { get => _author; set => _author = SetAuthor(value); }
    public string Content { get => _content; set => _content = SetContent(value); }

    private string _author;
    private string _content;

    public Comment(Guid id, string author, string content) : base(id)
    { 
        Author = author;
        Content = content;
    }
    public Comment(string author, string content)
    { 
        Author = author;
        Content = content;
    }

    public Comment() { }

    private static string SetAuthor(string author)
    {
        if (string.IsNullOrWhiteSpace(author))
            throw new ArgumentException("Author must contain a value", nameof(author));

        if (author.Length > 256)
            throw new ArgumentException("Author max length is 256", nameof(author));

        return author;
    }

    private static string SetContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Content must contain a value", nameof(content));

        if (content.Length > 256)
            throw new ArgumentException("Content max length is 512", nameof(content));

        return content;
    }
}
