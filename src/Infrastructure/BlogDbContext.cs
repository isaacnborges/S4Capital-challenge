using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using S4Capital.Api.Domain.Entities;

namespace S4Capital.Api.Infrastructure;

public class BlogDbContext : IdentityDbContext
{
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }

    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder
            .Entity<IdentityUserLogin<string>>()
            .HasKey(login => new { login.UserId, login.LoginProvider, login.ProviderKey });

        builder
            .Entity<IdentityUserRole<string>>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        builder.Seed();

        ModelPostEntity(builder);
        ModelCommentEntity(builder);

        base.OnModelCreating(builder);
    }

    private static void ModelPostEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Post>().ToTable("Posts");

        modelBuilder.Entity<Post>().HasKey(p => p.Id);
        modelBuilder.Entity<Post>().Property(p => p.Title).IsRequired();
        modelBuilder.Entity<Post>().Property(p => p.Content).IsRequired();
        modelBuilder.Entity<Post>().Property(p => p.Status).IsRequired();

        modelBuilder.Entity<Post>().Property(p => p.CreatedDate).IsRequired();

        modelBuilder.Entity<Post>().HasOne(p => p.Author).WithMany().HasForeignKey(p => p.CreatedBy);
        modelBuilder.Entity<Post>().HasOne(p => p.Editor).WithMany().HasForeignKey(p => p.EditedBy);
        modelBuilder.Entity<Post>().HasOne(p => p.Submitter).WithMany().HasForeignKey(p => p.SubmittedBy);
        modelBuilder.Entity<Post>().HasOne(p => p.Approver).WithMany().HasForeignKey(p => p.ApprovedBy);
        modelBuilder.Entity<Post>().HasOne(p => p.Rejecter).WithMany().HasForeignKey(p => p.RejectBy);

        modelBuilder.Entity<Post>().HasMany(p => p.Comments).WithOne(c => c.Post).HasForeignKey(c => c.PostId);
    }

    private static void ModelCommentEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>().ToTable("Comments");

        modelBuilder.Entity<Comment>().HasKey(c => c.Id);
        modelBuilder.Entity<Comment>().Property(c => c.Text).IsRequired();
        modelBuilder.Entity<Comment>().Property(c => c.CommentDate).IsRequired();

        modelBuilder.Entity<Comment>().HasOne(c => c.Author).WithMany().HasForeignKey(c => c.AuthorId);
        modelBuilder.Entity<Comment>().HasOne(c => c.Post).WithMany(p => p.Comments).HasForeignKey(c => c.PostId);
    }
}