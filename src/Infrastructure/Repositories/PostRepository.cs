using Microsoft.EntityFrameworkCore;
using S4Capital.Api.Domain.Entities;

namespace S4Capital.Api.Infrastructure.Repositories;

public class PostRepository : IPostRepository
{
    private readonly BlogDbContext _dbContext;

    public PostRepository(BlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreatePostAsync(Post post)
    {
        await _dbContext.Posts.AddAsync(post);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdatePostAsync(Post post)
    {
        _dbContext.Posts.Update(post);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Post> GetPendingByIdAsync(Guid id)
    {
        return await _dbContext
            .Posts
            .SingleOrDefaultAsync(post =>
                post.Id == id &&
                post.Status == PostStatus.Pending);
    }

    public async Task<Post> GetPendingByIdAndAuthorAsync(Guid id, string authorId)
    {
        return await _dbContext
            .Posts
            .SingleOrDefaultAsync(post =>
                post.Id == id &&
                post.CreatedBy == authorId &&
                post.Status == PostStatus.Pending);
    }

    public async Task<Post> GetApprovedByIdAndAuthorAsync(Guid id, string authorId)
    {
        return await _dbContext
            .Posts
            .SingleOrDefaultAsync(post =>
                post.Id == id &&
                post.CreatedBy == authorId &&
                post.Status == PostStatus.Approved);
    }

    public async Task<Post> GetPublishedByIdAsync(Guid id)
    {
        return await _dbContext
            .Posts
            .SingleOrDefaultAsync(post =>
                post.Id == id &&
                post.Status == PostStatus.Published);
    }

    public async Task<IEnumerable<Post>> GetPostsByAuthorAsync(string writerId)
    {
        return await _dbContext
            .Posts
            .AsNoTracking()
            .Where(post => post.CreatedBy == writerId)
            .OrderByDescending(post => post.CreatedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Post>> GetPendingPosts()
    {
        return await _dbContext
            .Posts
            .AsNoTracking()
            .Where(post => post.Status == PostStatus.Pending)
            .Include(post => post.Author)
            .ToListAsync();
    }

    public async Task<IEnumerable<Post>> GetPublishedPosts()
    {
        return await _dbContext
            .Posts
            .AsNoTracking()
            .Where(post => post.Status == PostStatus.Published)
            .Include(post => post.Author)
            .Include(post => post.Comments)
                .ThenInclude(comment => comment.Author)
            .OrderByDescending(post => post.CreatedDate)
                .ThenBy(post => post.Comments.Min(comment => comment.CommentDate))
            .ToListAsync();
    }

    public async Task AddCommentAsync(Comment comment)
    {
        _dbContext.Comments.Add(comment);
        await _dbContext.SaveChangesAsync();
    }
}
