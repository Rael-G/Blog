using Blog.Domain;

namespace Blog.Persistance;

public class ArchiveRepository(ApplicationDbContext context) 
    : BaseRepository<Archive>(context), IArchiveRepository
{
}
