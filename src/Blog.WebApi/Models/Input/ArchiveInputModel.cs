using Blog.Application;

namespace Blog.WebApi;

public class ArchiveInputModel : IInputModel<ArchiveDto>
{
    public bool IsPublic { get; set; }

    public ArchiveInputModel() { }

    public ArchiveInputModel(bool isPublic)
    {
        IsPublic = isPublic;
    }

    public ArchiveDto InputToDto()
        => new (){Id = Guid.NewGuid(), IsPublic = IsPublic};

    public void InputToDto(ArchiveDto dto)
    {
        dto.IsPublic = IsPublic;
    }
}
