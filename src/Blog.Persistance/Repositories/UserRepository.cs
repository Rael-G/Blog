﻿using Blog.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Persistance;

public class UserRepository(ApplicationDbContext context)
    : BaseRepository<User>(context), IUserRepository
{
    public async Task<User?> GetByUserName(string userName)
        => await Context.Users.Where(u => u.UserName == userName).FirstOrDefaultAsync();

    public async Task<IEnumerable<Post>> GetPostPage(Guid id, int page, int quantity)
        => await Context.Posts
            .AsNoTracking()
            .Where(p => p.UserId == id)
            .Include(p => p.User)
            .Include(p => p.Tags)
                .ThenInclude(pt => pt.Tag)
            .OrderByDescending(p => p.CreatedTime)
            .Skip((page - 1) * quantity)
            .Take(quantity)
            .ToListAsync();

    public async Task<int> GetPostCount(Guid id)
        => await Context.PostTag
            .AsNoTracking()
            .Where(pt => pt.TagId == id)
            .CountAsync();
}