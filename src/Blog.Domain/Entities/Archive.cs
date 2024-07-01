
namespace Blog.Domain;

public class Archive : BaseEntity
{
    //not null or whitespace
    public string FileName { get; set; }

    //not null or whitespace
    public string ContentUrl { get; set; }

    //not null or whitespace
    public string ContentType { get; set; }

    public Stream? Stream { get; set; }

    public Guid OwnerId { get; set; }

    public bool IsPublic { get; set; }

    public Archive(Guid id, string fileName, string contentUrl, string contentType, Guid ownerId, bool isPublic) : base(id)
    {
        FileName = fileName;
        ContentUrl = contentUrl;
        ContentType = contentType;
        OwnerId = ownerId;
        IsPublic = isPublic;
    }
}
