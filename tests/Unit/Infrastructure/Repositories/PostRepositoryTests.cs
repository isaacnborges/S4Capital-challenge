using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using S4Capital.Api.Domain.Entities;
using S4Capital.Api.Infrastructure;
using S4Capital.Api.Infrastructure.Repositories;
using S4Capital.Tests.Support.Mocks.Dtos.Commands;
using S4Capital.Tests.Support.Mocks.Entities;

namespace S4Capital.Tests.Unit.Infrastructure.Repositories;
public class PostRepositoryTests
{
    private readonly PostRepository _repository;
    private readonly BlogDbContext _dbContext;
    private readonly UserManager<IdentityUser> _userManager;

    public PostRepositoryTests()
    {
        _dbContext = DbContextHelper.CreateDbContext();
        _repository = new PostRepository(_dbContext);

        var userStore = new UserStore<IdentityUser>(_dbContext);
        var passwordHasher = new PasswordHasher<IdentityUser>();

        _userManager = new UserManager<IdentityUser>(
                userStore,
                null,
                passwordHasher,
                new List<IUserValidator<IdentityUser>>(),
                new List<IPasswordValidator<IdentityUser>>(),
                default,
                new IdentityErrorDescriber(),
                default,
                new LoggerFactory().CreateLogger<UserManager<IdentityUser>>());
    }

    [Fact]
    public async Task CreatePostAsync_ValidPost_AddPostToDatabase()
    {
        // Arrange
        var post = PostMock.GetFaker();

        // Act
        await _repository.CreatePostAsync(post);

        // Assert
        var addedPost = await GetPostById(post);
        addedPost.Should().BeEquivalentTo(post);
    }

    [Fact]
    public async Task UpdatePostAsync_ValidPost_UpdatePostInDatabase()
    {
        // Arrange
        var post = PostMock.GetFaker();
        _dbContext.Posts.Add(post);
        await _dbContext.SaveChangesAsync();

        var command = EditPostCommandMock.GetFaker();
        post.Title = command.Title;
        post.Content = command.Content;

        // Act
        await _repository.UpdatePostAsync(post);

        // Assert
        var updatedPost = await GetPostById(post);
        updatedPost.Title.Should().Be(command.Title);
        updatedPost.Content.Should().Be(command.Content);
    }

    [Fact]
    public async Task GetPendingByIdAsync_ExistsPost_ShouldReturnPost()
    {
        // Arrange
        var post = PostMock.GetFaker();
        _dbContext.Posts.Add(post);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetPendingByIdAsync(post.Id);

        // Assert
        result.Should().BeEquivalentTo(post);
    }

    [Fact]
    public async Task GetPendingByIdAsync_NotExistsPendingPost_ShouldReturnNull()
    {
        // Arrange
        var post = PostMock.GetFaker();
        post.Status = PostStatus.Approved;
        _dbContext.Posts.Add(post);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetPendingByIdAsync(post.Id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetPendingByIdAndAuthorAsync_ExistsPost_ShouldReturnPost()
    {
        // Arrange
        var post = PostMock.GetFaker();
        _dbContext.Posts.Add(post);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetPendingByIdAndAuthorAsync(post.Id, post.CreatedBy);

        // Assert
        result.Should().BeEquivalentTo(post);
    }

    [Fact]
    public async Task GetPendingByIdAndAuthorAsync_NotExistsPendingPost_ShouldReturnNull()
    {
        // Arrange
        var post = PostMock.GetFaker();
        post.Status = PostStatus.Approved;
        _dbContext.Posts.Add(post);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetPendingByIdAndAuthorAsync(post.Id, Guid.NewGuid().ToString());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetApprovedByIdAndAuthorAsync_ExistsPost_ShouldReturnPost()
    {
        // Arrange
        var post = PostMock.GetFaker();
        post.Status = PostStatus.Approved;
        _dbContext.Posts.Add(post);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetApprovedByIdAndAuthorAsync(post.Id, post.CreatedBy);

        // Assert
        result.Should().BeEquivalentTo(post);
    }

    [Fact]
    public async Task GetApprovedByIdAndAuthorAsync_NotExistsPendingPost_ShouldReturnNull()
    {
        // Arrange
        var post = PostMock.GetFaker();
        post.Status = PostStatus.Approved;
        _dbContext.Posts.Add(post);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetApprovedByIdAndAuthorAsync(post.Id, Guid.NewGuid().ToString());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetPublishedByIdAsync_ExistsPost_ShouldReturnPost()
    {
        // Arrange
        var post = PostMock.GetFaker();
        post.Status = PostStatus.Published;
        _dbContext.Posts.Add(post);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetPublishedByIdAsync(post.Id);

        // Assert
        result.Should().BeEquivalentTo(post);
    }

    [Fact]
    public async Task GetPublishedByIdAsync_NotExistsPendingPost_ShouldReturnNull()
    {
        // Arrange
        var postId = Guid.NewGuid();

        // Act
        var result = await _repository.GetPublishedByIdAsync(postId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetPostsByAuthorAsync_ExistsPosts_ShouldReturnPosts()
    {
        // Arrange
        var totalPosts = 10;
        var expectedPosts = 2;

        var userId = Guid.NewGuid().ToString();
        var posts = PostMock.GetListFaker(totalPosts);
        var identityUser = GetIdentityUser(userId);

        await _userManager.CreateAsync(identityUser, "Password123");
        var user = await _userManager.FindByIdAsync(userId)!;

        posts[0].CreatedBy = user.Id;
        posts[0].Author = user;

        posts[1].CreatedBy = user.Id;
        posts[1].Author = user;


        await _dbContext.Posts.AddRangeAsync(posts);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetPostsByAuthorAsync(userId);

        // Assert
        result.Should().HaveCount(expectedPosts);
        result.Should().OnlyContain(x => x.CreatedBy == userId);
    }

    [Fact]
    public async Task GetPendingPosts_ExistsPosts_ShouldReturnPosts()
    {
        // Arrange
        var totalPosts = 10;
        var expectedPosts = 9;

        var posts = PostMock.GetListFaker(totalPosts);
        posts[0].Status = PostStatus.Published;

        await _dbContext.Posts.AddRangeAsync(posts);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetPendingPosts();

        // Assert
        result.Should().HaveCount(expectedPosts);
        result.Should().OnlyContain(x => x.Status == PostStatus.Pending);
    }

    [Fact]
    public async Task GetPublishedPosts_ExistsPosts_ShouldReturnPosts()
    {
        // Arrange
        var totalPosts = 5;
        var expectedPosts = 1;

        var posts = PostMock.GetListFaker(totalPosts);
        var comment = new Comment(AddCommentCommandMock.GetFaker(), Guid.NewGuid().ToString());
        posts[0].Status = PostStatus.Published;
        posts[0].Comments.Add(comment);

        await _dbContext.Posts.AddRangeAsync(posts);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetPublishedPosts();

        // Assert
        result.Should().HaveCount(expectedPosts);
        result.Should().OnlyContain(x => x.Status == PostStatus.Published);
    }

    [Fact]
    public async Task AddCommentAsync_ExistsPublishedPosts_ShouldAddComment()
    {
        // Arrange
        var post = PostMock.GetFakerPublished();
        var comment = new Comment(AddCommentCommandMock.GetFaker(), Guid.NewGuid().ToString());
        comment.PostId = post.Id;

        await _dbContext.Posts.AddAsync(post);
        await _dbContext.SaveChangesAsync();

        // Act
        await _repository.AddCommentAsync(comment);

        // Assert
        var addedPost = await GetPostById(post);
        addedPost.Comments.Should().NotBeEmpty();
    }

    private static IdentityUser GetIdentityUser(string userId)
    {
        return new IdentityUser
        {
            Id = userId,
            UserName = "user1",
            Email = "user1@example.com"
        };
    }

    private async Task<Post?> GetPostById(Post post)
    {
        return await _dbContext
            .Posts
            .Include(x => x.Comments)
            .FirstOrDefaultAsync(x => x.Id == post.Id);
    }

    private static class DbContextHelper
    {
        public static BlogDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<BlogDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new BlogDbContext(options);
        }
    }
}