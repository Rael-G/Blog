using Blog.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Persistance;

public class UserRepository(ApplicationDbContext context)
    : BaseRepository<User>(context), IUserRepository
{
    public async Task<User?> GetByUserName(string userName)
        => await Context.Users.Where(u => u.UserName == userName).FirstOrDefaultAsync();
}