using Blog.Domain;
using Blog.Domain.Entities;

namespace Blog.Infrastructure.Persistance;

public class PostRepository(ApplicationDbContext context) 
    : BaseRepository<Post>(context), IPostRepository
{

}
