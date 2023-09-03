using CustomExceptionMiddleware.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using S4Capital.Api.Api.Dtos.Commands;
using S4Capital.Api.Api.Dtos.Responses;
using S4Capital.Api.Core.Extensions;
using S4Capital.Api.Domain.Entities;
using S4Capital.Api.Infrastructure;
using S4Capital.Tests.Support;
using S4Capital.Tests.Support.Mocks.Dtos.Commands;
using S4Capital.Tests.Support.Mocks.Entities;
using System.Net.Http.Json;

namespace S4Capital.Tests.Integration.Controllers;
public class PostsControllerTests : IClassFixture<IntegrationTestWebAppFactory>, IDisposable
{
    private readonly IntegrationTestWebAppFactory _factory;
    private readonly IServiceScope _scope;
    protected readonly BlogDbContext _dbContext;
    private readonly UserManager<IdentityUser> _userManager;

    private string _requestUri = "/posts/";

    public PostsControllerTests(IntegrationTestWebAppFactory factory)
    {
        _factory = factory;
        _scope = factory.Services.CreateScope();
        _dbContext = _scope.ServiceProvider.GetRequiredService<BlogDbContext>();

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

        CreateUser().Wait();
    }

    [Fact]
    public async Task GetAllPublishedPosts_ValidRequest_ShouldReturnOk()
    {
        // Arrange
        var client = _factory.CreateClient();
        var totalPosts = 5;
        var posts = PostMock.GetListFakerPublished(totalPosts);

        SeedPost(posts.ToArray());

        // Act
        var response = await client.GetAsync(_requestUri);

        // Assert
        response.Should().Be200Ok();

        var postsResponse = await response.Content.ReadFromJsonAsync<List<PublishedPostResponse>>();
        postsResponse.Should().HaveCount(totalPosts);
    }

    [Fact]
    public async Task AddCommentToPost_ValidRequest_ShouldReturnNoContent()
    {
        // Arrange
        var client = _factory.CreateClient();
        var post = PostMock.GetFakerPublished();
        var command = AddCommentCommandMock.GetFaker();

        _requestUri += $"{post.Id}/comment";

        SeedPost(post);

        // Act
        var response = await client.PostAsJsonAsync(_requestUri, command);

        // Assert
        response.Should().Be204NoContent();

        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
        var postDb = GetPostById(context, post.Id);
        postDb.Comments.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetPostsByWriter_ValidRequest_ShouldReturnOk()
    {
        // Arrange
        var client = _factory.CreateClient();
        var totalPosts = 5;
        var expectedPosts = 2;
        var posts = PostMock.GetListFaker(totalPosts);

        var user = await _userManager.FindByIdAsync(ClaimsPrincipalMock.DefaultUserId)!;

        posts[0].CreatedBy = user.Id;
        posts[0].Author = user;

        posts[1].CreatedBy = user.Id;
        posts[1].Author = user;

        _requestUri += $"writer";

        SeedPost(posts.ToArray());

        // Act
        var response = await client.GetAsync(_requestUri);

        // Assert
        response.Should().Be200Ok();

        var postsResponse = await response.Content.ReadFromJsonAsync<List<PostByWriterResponse>>();
        postsResponse.Should().HaveCount(expectedPosts);
    }

    [Fact]
    public async Task CreatePost_ValidRequest_ShouldReturnCreated()
    {
        // Arrange
        var client = _factory.CreateClient();
        var command = CreatePostCommandMock.GetFaker();

        // Act
        var response = await client.PostAsJsonAsync(_requestUri, command);

        // Assert
        response.Should().Be201Created();

        var postResponse = await response.Content.ReadFromJsonAsync<CreatePostResponse>();
        postResponse.Should().NotBeNull();
    }

    [Fact]
    public async Task CreatePost_InvalidValidRequest_ShouldReturnBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();
        var command = new CreatePostCommand
        {
            Title = null,
            Content = null
        };
        var expectedErrors = new CustomErrorResponse
        {
            Type = CustomExceptionMiddleware.CustomExceptionMiddleware.ValidationErrors,
            Errors = new List<string> {
                "[Title] must be informed",
                "[Content] must be informed"
            }
        };

        // Act
        var response = await client.PostAsJsonAsync(_requestUri, command);

        // Assert
        response.Should().Be400BadRequest();

        var errorResponse = await response.Content.ReadFromJsonAsync<CustomErrorResponse>();
        errorResponse.Should().BeEquivalentTo(expectedErrors);
    }

    [Fact]
    public async Task EditPost_ValidRequest_ShouldReturnNoContent()
    {
        // Arrange
        var client = _factory.CreateClient();
        var command = EditPostCommandMock.GetFaker();
        var post = PostMock.GetFaker();
        var user = await _userManager.FindByIdAsync(ClaimsPrincipalMock.DefaultUserId)!;
        post.CreatedBy = user.Id;
        post.Author = user;

        _requestUri += $"{post.Id}";

        SeedPost(post);

        // Act
        var response = await client.PutAsJsonAsync(_requestUri, command);

        // Assert
        response.Should().Be204NoContent();

        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
        var postDb = GetPostById(context, post.Id);
        postDb.Title.Should().Be(command.Title);
        postDb.Content.Should().Be(command.Content);
    }

    [Fact]
    public async Task EditPost_InvalidValidRequest_ShouldReturnBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();
        var command = new EditPostCommand
        {
            Title = null,
            Content = null
        };
        var expectedErrors = new CustomErrorResponse
        {
            Type = CustomExceptionMiddleware.CustomExceptionMiddleware.ValidationErrors,
            Errors = new List<string> {
                "[Title] must be informed",
                "[Content] must be informed"
            }
        };

        _requestUri += $"{Guid.NewGuid()}";

        // Act
        var response = await client.PutAsJsonAsync(_requestUri, command);

        // Assert
        response.Should().Be400BadRequest();

        var errorResponse = await response.Content.ReadFromJsonAsync<CustomErrorResponse>();
        errorResponse.Should().BeEquivalentTo(expectedErrors);
    }

    [Fact]
    public async Task SubmitPost_ValidRequest_ShouldReturnNoContent()
    {
        // Arrange
        var client = _factory.CreateClient();
        var command = SubmitPostCommandMock.GetFaker();
        var post = PostMock.GetFaker();
        post.Status = PostStatus.Approved;

        var user = await _userManager.FindByIdAsync(ClaimsPrincipalMock.DefaultUserId)!;
        post.CreatedBy = user.Id;
        post.Author = user;

        _requestUri += $"{post.Id}/submit";

        SeedPost(post);

        // Act
        var response = await client.PatchAsJsonAsync(_requestUri, command);

        // Assert
        response.Should().Be204NoContent();

        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
        var postDb = GetPostById(context, post.Id);
        postDb.Status.Should().Be(PostStatus.Published);
        postDb.SubmittedBy.Should().NotBeEmpty();
        postDb.SubmittedDate.Should().NotBeNull();
    }

    [Fact]
    public async Task GetPendingPosts_ValidRequest_ShouldReturnOk()
    {
        // Arrange
        var client = _factory.CreateClient();
        var totalPosts = 5;
        var posts = PostMock.GetListFaker(totalPosts);

        SeedPost(posts.ToArray());

        _requestUri += "pending-posts";

        // Act
        var response = await client.GetAsync(_requestUri);

        // Assert
        response.Should().Be200Ok();

        var postsResponse = await response.Content.ReadFromJsonAsync<List<PendingPostResponse>>();
        postsResponse.Should().HaveCountGreaterThanOrEqualTo(totalPosts);
        postsResponse.Should().OnlyContain(x => x.Status == PostStatus.Pending.GetDescription());
    }

    [Fact]
    public async Task ApprovePost_ValidRequest_ShouldReturnNoContent()
    {
        // Arrange
        var client = _factory.CreateClient();
        var command = SubmitPostCommandMock.GetFaker();
        var post = PostMock.GetFaker();
        var user = await _userManager.FindByIdAsync(ClaimsPrincipalMock.DefaultUserId)!;
        post.CreatedBy = user.Id;
        post.Author = user;

        _requestUri += $"{post.Id}/approve";

        SeedPost(post);

        // Act
        var response = await client.PatchAsJsonAsync(_requestUri, command);

        // Assert
        response.Should().Be204NoContent();

        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
        var postDb = GetPostById(context, post.Id);
        postDb.Status.Should().Be(PostStatus.Approved);
        postDb.ApprovedBy.Should().NotBeEmpty();
        postDb.ApprovedDate.Should().NotBeNull();
    }

    [Fact]
    public async Task RejectPost_ValidRequest_ShouldReturnNoContent()
    {
        // Arrange
        var client = _factory.CreateClient();
        var command = SubmitPostCommandMock.GetFaker();
        var post = PostMock.GetFaker();
        var user = await _userManager.FindByIdAsync(ClaimsPrincipalMock.DefaultUserId)!;
        post.CreatedBy = user.Id;
        post.Author = user;

        _requestUri += $"{post.Id}/reject";

        SeedPost(post);

        // Act
        var response = await client.PatchAsJsonAsync(_requestUri, command);

        // Assert
        response.Should().Be204NoContent();

        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
        var postDb = GetPostById(context, post.Id);
        postDb.Status.Should().Be(PostStatus.Rejected);
        postDb.RejectBy.Should().NotBeEmpty();
        postDb.RejectDate.Should().NotBeNull();
    }

    private async Task CreateUser()
    {
        var user = await _userManager.FindByIdAsync(ClaimsPrincipalMock.DefaultUserId);
        if (user is null)
        {
            var userId = ClaimsPrincipalMock.DefaultUserId;
            var identityUser = GetIdentityUser(userId);
            await _userManager.CreateAsync(identityUser, "Password123");
        }
    }

    private void SeedPost(params Post[] posts)
    {
        _dbContext.Posts.AddRange(posts);
        _dbContext.SaveChanges();
    }

    private Post GetPostById(BlogDbContext context, Guid postId)
    {
        return context
            .Posts
            .Include(x => x.Comments)
            .First(x => x.Id == postId);
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

    public void Dispose()
    {
        _scope?.Dispose();
        _dbContext?.Dispose();
    }
}

public record CustomErrorResponse
{
    public string Type { get; set; }
    public List<string> Errors { get; set; }
}
