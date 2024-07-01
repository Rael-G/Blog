namespace Blog.Application;

public class ArchiveDto : IDto
{
    public Guid Id { get; set; }

    public string FileName { get; set; } = string.Empty;

    public string ContentUrl { get; set; } = string.Empty;

    public string ContentType { get; set; } = string.Empty;

    public Stream? Stream { get; set; }

    public Guid OwnerId { get; set; }

    public bool IsPublic { get; set; }
}
