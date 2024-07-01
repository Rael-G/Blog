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
        return Task.Run(() => 
        {
            if (File.Exists(Path.Combine(_path, fullPath)))
                return (Stream) new FileStream(fullPath, FileMode.Open);
        
            return null;
        });
    }

    public async Task<string> StoreAsync(Stream file)
    {
        string filePath = Guid.NewGuid().ToString();
        string fullPath = Path.Combine(_path, filePath);
        using var fileStream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(fileStream);

        return filePath;
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
