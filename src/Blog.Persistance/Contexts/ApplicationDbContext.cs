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

        ConfigureBaseEntity<Post>(modelBuilder);
        modelBuilder.Entity<Post>(post =>
        {
            post.Property(p => p.Title).IsRequired().HasMaxLength(256);
            post.Property(p => p.Content).IsRequired();
            post.HasIndex(p => p.Title).IsUnique();
        });

        ConfigureBaseEntity<Comment>(modelBuilder);
        modelBuilder.Entity<Comment>(comment =>
        {
            comment.Property(c => c.Author).IsRequired().HasMaxLength(256);
            comment.Property(c => c.Content).IsRequired().HasMaxLength(512);
            comment.HasOne(c => c.Post).WithMany(p => p.Comments).HasForeignKey(c => c.PostId).IsRequired();
        });

        ConfigureBaseEntity<Tag>(modelBuilder);
        modelBuilder.Entity<Tag>(tag => 
        {
            tag.Property(t => t.Name).IsRequired();
            tag.HasIndex(t => t.Name).IsUnique();
        });
    }

    private void ConfigureBaseEntity<T>(ModelBuilder modelBuilder) where T : BaseEntity
    {
        modelBuilder.Entity<T>(entity => {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedTime).IsRequired().ValueGeneratedOnAdd();
            entity.Property(e=> e.ModifiedTime).IsRequired();
        });
    }
}