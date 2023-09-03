using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Moq.AutoMock;
using S4Capital.Api.Api.Dtos.Queries;
using S4Capital.Api.Api.Dtos.Responses;
using S4Capital.Api.Domain.Entities;
using S4Capital.Api.Domain.QueryHandlers;
using S4Capital.Api.Infrastructure.Repositories;
using S4Capital.Tests.Support;
using S4Capital.Tests.Support.Mocks.Entities;
using System.Security.Claims;

namespace S4Capital.Tests.Unit.Domain.QueryHandlers;
public class PostsByWriterQueryHandlerTests
{
    private readonly AutoMocker _mocker;
    private readonly PostsByWriterQueryHandler _queryHandler;

    public PostsByWriterQueryHandlerTests()
    {
        _mocker = new AutoMocker();
        _queryHandler = _mocker.CreateInstance<PostsByWriterQueryHandler>();
    }

    [Fact]
    public async Task Handle_ExistsPosts_ShouldReturnPostsByWriter()
    {
        // Arrange
        var postsQuantity = 5;
        var posts = PostMock.GetListFaker(postsQuantity);
        var query = new PostsByWriterQuery();

        _mocker.GetMock<IHttpContextAccessor>()
            .Setup(x => x.HttpContext!.User)
            .Returns(new ClaimsPrincipalMock(new Claim(ClaimTypes.NameIdentifier, ClaimsPrincipalMock.DefaultUserId)));

        _mocker.GetMock<IPostRepository>()
            .Setup(x => x.GetPostsByAuthorAsync(ClaimsPrincipalMock.DefaultUserId))
            .ReturnsAsync(posts);

        // Act
        var result = await _queryHandler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeOfType<List<PostByWriterResponse>>();
        result.Should().HaveCount(postsQuantity);
    }

    [Fact]
    public async Task Handle_NotExistsPostsByWriter_ShouldReturnEmptyList()
    {
        // Arrange
        var query = new PostsByWriterQuery();

        _mocker.GetMock<IHttpContextAccessor>()
            .Setup(x => x.HttpContext!.User)
            .Returns(new ClaimsPrincipalMock(new Claim(ClaimTypes.NameIdentifier, ClaimsPrincipalMock.DefaultUserId)));

        _mocker.GetMock<IPostRepository>()
            .Setup(x => x.GetPostsByAuthorAsync(ClaimsPrincipalMock.DefaultUserId))
            .ReturnsAsync(Enumerable.Empty<Post>());

        // Act
        var result = await _queryHandler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeOfType<List<PostByWriterResponse>>();
        result.Should().BeEmpty();
    }
}
