using Blog.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Persistance;

public class UserRepository(ApplicationDbContext context)
    : BaseRepository<User>(context), IUserRepository
{
    public async Task<User?> GetByUserName(string userName)
        => await Context.Users.Where(u => u.UserName == userName).FirstOrDefaultAsync();

    public async Task<IEnumerable<Post>> GetPostPage(Guid id, int page, int quantity)
        => await Context.PostTag
            .AsNoTracking()
            .Where(pt => pt.TagId == id)
            .Include(pt => pt.Post!)
                .ThenInclude(p => p.Tags)
                    .ThenInclude(t => t.Tag)
            .OrderByDescending(p => p.Post!.CreatedTime)
            .Skip((page - 1) * quantity)
            .Take(quantity)
            .Select(pt => pt.Post!)
            .ToListAsync();

    public async Task<int> GetPostCount(Guid id)
        => await Context.PostTag
            .AsNoTracking()
            .Where(pt => pt.TagId == id)
            .CountAsync();
}