using Blog.Application;

namespace Blog.WebApi;

public class ArchiveOutputModel
{
    public Guid Id { get; set; }

    public string FileName { get; set; } = string.Empty;

    public string ContentUrl { get; set; } = string.Empty;

    public string ContentType { get; set; } = string.Empty;

    public Guid OwnerId { get; set; }

    public bool IsPublic { get; set; }

    public ArchiveOutputModel(){ }

    public ArchiveOutputModel(ArchiveDto archive)
    {
        Id = archive.Id;
        FileName = archive.FileName;
        ContentUrl = archive.ContentUrl;
        ContentType = archive.ContentType;
        OwnerId = archive.OwnerId;
        IsPublic = archive.IsPublic;
    }

    public static IEnumerable<ArchiveOutputModel> MapRange(IEnumerable<ArchiveDto> archiveDtos)
    {
        List<ArchiveOutputModel> archiveOutputs = [];
        foreach (var archiveDto in archiveDtos)
        {
            archiveOutputs.Add(new ArchiveOutputModel(archiveDto));
        }

        return archiveOutputs;
    }
}