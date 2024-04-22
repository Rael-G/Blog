using Blog.Domain;

namespace Blog.Persistance;

public class UserRepository(ApplicationDbContext context) 
    : BaseRepository<User>(context), IUserRepository
{
    
}