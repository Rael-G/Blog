namespace Blog.Domain;

public class Comment
    : BaseEntity
{
    public string Author { get; set; }
    public string Content { get; set; }


    public Comment(Guid id, string author, string content) : base(id)
    { 
        Author = author;
        Content = content;
    }
    public Comment(string author, string content) : base(null)
    { 
        Author = author;
        Content = content;
    }
}
