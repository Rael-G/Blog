using Blog.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Persistance;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
    : DbContext(options)
{
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Post>(post =>
        {
            post.HasKey(p => p.Id);
            post.Property(p => p.CreatedTime).IsRequired();
            post.Property(p => p.UpdateTime).IsRequired();
            post.Property(p => p.Title).IsRequired().HasMaxLength(256);
            post.Property(p => p.Content).IsRequired();
            post.HasMany(p => p.Comments).WithOne(c => c.Post).HasForeignKey(c => c.PostId);
        });

        modelBuilder.Entity<Comment>(comment =>
        {
            comment.HasKey(c => c.Id);
            comment.Property(c => c.CreatedTime).IsRequired();
            comment.Property(c => c.UpdateTime).IsRequired();
            comment.Property(c => c.Author).IsRequired().HasMaxLength(256);
            comment.Property(c => c.Content).IsRequired().HasMaxLength(512);
        });
    }
}
