using Blog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Blog.Persistance;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<PostTag> PostTag { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureBaseEntity<User>(modelBuilder);
        modelBuilder.Entity<User>(user =>
        {
            user.Property(u => u.UserName)
                .IsRequired();
            user.Property(u => u.PasswordHash)
                .IsRequired();
            user.Property(u => u.Roles)
                .IsRequired();
            user.HasIndex(u => u.UserName)
                .IsUnique();
        });

        ConfigureBaseEntity<Post>(modelBuilder);
        modelBuilder.Entity<Post>(post =>
        {
            post.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(256);
            post.Property(p => p.Content)
                .IsRequired();
            post.HasIndex(p => p.Title)
                .IsUnique();
            post.HasIndex(p => p.CreatedTime);
            post.HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .IsRequired();
        });

        ConfigureBaseEntity<Comment>(modelBuilder);
        modelBuilder.Entity<Comment>(comment =>
        {
            comment.Property(c => c.Author)
                .IsRequired()
                .HasMaxLength(256);
            comment.Property(c => c.Content)
                .IsRequired()
                .HasMaxLength(512);
            comment.HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .IsRequired();
        });

        ConfigureBaseEntity<Tag>(modelBuilder);
        modelBuilder.Entity<Tag>(tag =>
        {
            tag.Property(t => t.Name)
                .IsRequired();
            tag.HasIndex(t => t.Name)
                .IsUnique();
        });

        modelBuilder.Entity<PostTag>(postTag =>
        {
            postTag.HasKey(pt => new { pt.PostId, pt.TagId });
            postTag.HasOne(pt => pt.Post)
                .WithMany(p => p.Tags)
                .HasForeignKey(pt => pt.PostId);
            postTag.HasOne(pt => pt.Tag)
                .WithMany(t => t.Posts)
                .HasForeignKey(pt => pt.TagId);
        });
    }

    private static void ConfigureBaseEntity<T>(ModelBuilder modelBuilder) where T : BaseEntity
    {
        modelBuilder.Entity<T>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedTime)
                .IsRequired()
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            entity.Property(e => e.ModifiedTime)
                .IsRequired();
        });
    }
}