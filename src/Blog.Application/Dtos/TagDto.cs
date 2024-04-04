namespace Blog.Application;

public class TagDto : IDto
{
    public Guid Id { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime ModifiedTime { get; set; }
    public string Name { get; set; }
    public IEnumerable<PostDto> Posts { get; set; }

    public TagDto(Guid id, string name, IEnumerable<PostDto> posts)
    {
        Id = id;
        Name = name;
        Posts = posts;
    }
}