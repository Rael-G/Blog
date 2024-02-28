using Blog.Domain;

namespace Blog.Persistance;

public class PostRepository(ApplicationDbContext context) 
    : BaseRepository<Post>(context), IPostRepository
{

}
