using Blog.Domain;
using Microsoft.AspNetCore.Http;

namespace Blog.Application;

public interface IArchiveService : IBaseService<ArchiveDto>
{
    Task<ArchiveDto?> GetWithFile(ArchiveDto archive);

    Task Create(ArchiveDto dto, IFormFile file);

    Task Update(ArchiveDto dto, IFormFile file);
}
