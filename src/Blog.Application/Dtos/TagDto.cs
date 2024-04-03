namespace Blog.Application;

public class TagDto
{
    public Guid Id { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime ModifiedTime { get; set; }
    public string Name { get; set; }
    public IEnumerable<PostDto> Posts { get; set; }

    public TagDto(Guid id, DateTime createdTime, DateTime modifiedTime, string name, IEnumerable<PostDto> posts)
    {
        Id = id;
        CreatedTime = createdTime;
        ModifiedTime = modifiedTime;
        Name = name;
        Posts = posts;
    }
}