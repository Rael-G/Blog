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
            post.Property(p => p.CreatedTime).IsRequired().ValueGeneratedOnAdd();
            post.Property(p => p.ModifiedTime).IsRequired();
            post.Property(p => p.Title).IsRequired().HasMaxLength(256);
            post.Property(p => p.Content).IsRequired();
        });

        modelBuilder.Entity<Comment>(comment =>
        {
            comment.HasKey(c => c.Id);
            comment.Property(c => c.CreatedTime).IsRequired().ValueGeneratedOnAdd();
            comment.Property(c => c.ModifiedTime).IsRequired();
            comment.Property(c => c.Author).IsRequired().HasMaxLength(256);
            comment.Property(c => c.Content).IsRequired().HasMaxLength(512);
            comment.HasOne(c => c.Post).WithMany(p => p.Comments).HasForeignKey(c => c.PostId).IsRequired();
        });
    }
}
