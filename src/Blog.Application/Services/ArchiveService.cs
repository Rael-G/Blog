using AutoMapper;
using Blog.Domain;
using Microsoft.AspNetCore.Http;

namespace Blog.Application;

public class ArchiveService(IArchiveRepository ArchiveRepository, IMapper Mapper, IFileStorage FileStorage) 
: BaseService<ArchiveDto, Archive>(ArchiveRepository, Mapper), IArchiveService
{
    public async Task<ArchiveDto?> GetWithFile(ArchiveDto archive)
    {
        archive.Stream = await FileStorage.GetByPathAsync(archive.ContentUrl);
        return Mapper.Map<ArchiveDto>(archive);
    }

    public async Task Create(ArchiveDto dto, IFormFile file)
    {
        dto.Stream = file.OpenReadStream();
        dto.ContentUrl = await FileStorage.StoreAsync(dto.Stream);
        dto.FileName = file.FileName;
        dto.ContentType = file.ContentType;

        var archive = Mapper.Map<Archive>(dto);

        try
        {
            ArchiveRepository.Create(archive);
            await ArchiveRepository.Commit();
        }
        catch
        {
            await FileStorage.DeleteAsync(archive.ContentUrl);
            throw;
        }
        
    }

    public async Task Update(ArchiveDto dto, IFormFile file)
    {
        var oldUrl = (await ArchiveRepository.Get(dto.Id))?.ContentUrl ?? throw new AppException("Archive not found.");
        
        var archive = Mapper.Map<Archive>(dto);

        var stream = file.OpenReadStream();
        archive.ContentUrl = await FileStorage.StoreAsync(stream);
        archive.ContentType = file.ContentType;
        archive.FileName = file.FileName;

        try
        {
            ArchiveRepository.Update(archive);
            await ArchiveRepository.Commit();
        }
        catch
        {
            await FileStorage.DeleteAsync(archive.ContentUrl);
            throw;
        }

        await FileStorage.DeleteAsync(oldUrl);
    }

    public async override Task Delete(ArchiveDto dto)
    {
        var archive = Mapper.Map<Archive>(dto);
        ArchiveRepository.Delete(archive);
        await ArchiveRepository.Commit();
        await FileStorage.DeleteAsync(archive.ContentUrl);
    }
}
