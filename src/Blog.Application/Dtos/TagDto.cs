namespace Blog.Application;

public class TagDto
{
    public string Name { get; set; }
    public IEnumerable<PostDto> Posts { get; set; }

    public TagDto(string name, IEnumerable<PostDto> posts)
    {
        Name = name;
        Posts = posts;
    }
}