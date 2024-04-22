using Blog.Domain;

namespace Blog.Persistance;

public class UserRepository(ApplicationDbContext context)
    : BaseRepository<User>(context), IUserRepository
{
    public Task<User> GetByUserName(string userName)
    {
        throw new NotImplementedException();
    }
}