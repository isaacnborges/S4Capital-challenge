using S4Capital.Api.Domain.Entities;

namespace S4Capital.Api.Infrastructure.Repositories;

public interface IPostRepository
{
    Task CreatePostAsync(Post post);
    Task UpdatePostAsync(Post post);
    Task<Post> GetPendingByIdAsync(Guid id);
    Task<Post> GetPendingByIdAndAuthorAsync(Guid id, string authorId);
    Task<Post> GetApprovedByIdAndAuthorAsync(Guid id, string authorId);
    Task<Post> GetPublishedByIdAsync(Guid id);
    Task<IEnumerable<Post>> GetPostsByAuthorAsync(string writerId);
    Task<IEnumerable<Post>> GetPendingPosts();
    Task<IEnumerable<Post>> GetPublishedPosts();
    Task AddCommentAsync(Comment comment);
}
