using Blog.Domain;

namespace Blog.Persistance;

public class LocalFileStorage : IFileStorage
{
    private readonly string _path;

    public LocalFileStorage(string path)
    {
        _path = path;
        if (!Directory.Exists(_path)) Directory.CreateDirectory(_path);
    }

    public Task<Stream?> GetByPathAsync(string fullPath)
    {
        if (File.Exists(fullPath))
            return Task.FromResult<Stream?>(new FileStream(fullPath, FileMode.Open));
        
        return Task.FromResult<Stream?>(null);
    }

    public async Task<string> StoreAsync(Stream file)
    {
        Guid fileName = Guid.NewGuid();
        string fullPath = Path.Combine(_path, fileName.ToString());
        using var fileStream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(fileStream);

        return fullPath;
    }

    public async Task<bool> DeleteAsync(string path)
    {
        bool result;
        try
        {
            await Task.Run(() => File.Delete(path));
        }
        finally 
        { 
            result = !File.Exists(path);
        }

        return result;
    }
}
