using Blog.Domain;

namespace Blog.Infrastructure.Persistance;

public class PostRepository(ApplicationDbContext context) 
    : BaseRepository<Post>(context), IPostRepository
{

}
