using Blog.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Persistance;

public class PostRepository(ApplicationDbContext context)
    : BaseRepository<Post>(context), IPostRepository
{
    public override async Task<Post?> Get(Guid id)
        => await Context.Posts
        .AsNoTracking()
        .Include(p => p.Comments)
        .Include(p => p.Tags)
        .ThenInclude(pt => pt.Tag)
        .FirstOrDefaultAsync(p => p.Id == id);

    public async Task UpdatePostTag(Post post) {

        var existentPostTags = await Context.PostTag.Where(pt => pt.PostId == post.Id).ToListAsync();
        var postTagsToDelete = existentPostTags.Except(post.Tags);
        var postTagsToCreate = post.Tags.Except(existentPostTags);
        Context.PostTag.RemoveRange(postTagsToDelete);
        Context.PostTag.AddRange(postTagsToCreate);

        base.Update(post);
    }
}