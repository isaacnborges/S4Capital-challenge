using FluentAssertions;
using Moq;
using Moq.AutoMock;
using S4Capital.Api.Api.Dtos.Queries;
using S4Capital.Api.Api.Dtos.Responses;
using S4Capital.Api.Domain.Entities;
using S4Capital.Api.Domain.QueryHandlers;
using S4Capital.Api.Infrastructure.Repositories;
using S4Capital.Tests.Support.Mocks.Entities;

namespace S4Capital.Tests.Unit.Domain.QueryHandlers;
public class PendingPostsQueryHandlerTests
{
    private readonly AutoMocker _mocker;
    private readonly PendingPostsQueryHandler _queryHandler;

    public PendingPostsQueryHandlerTests()
    {
        _mocker = new AutoMocker();
        _queryHandler = _mocker.CreateInstance<PendingPostsQueryHandler>();
    }

    [Fact]
    public async Task Handle_ExistsPosts_ShouldReturnPendingPosts()
    {
        // Arrange
        var postsQuantity = 5;
        var posts = PostMock.GetListFaker(postsQuantity);
        var query = new PendingPostsQuery();

        _mocker.GetMock<IPostRepository>()
            .Setup(x => x.GetPendingPosts())
            .ReturnsAsync(posts);

        // Act
        var result = await _queryHandler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeOfType<List<PendingPostResponse>>();
        result.Should().HaveCount(postsQuantity);
    }

    [Fact]
    public async Task Handle_NotExistsPendingPosts_ShouldReturnEmptyList()
    {
        // Arrange
        var query = new PendingPostsQuery();

        _mocker.GetMock<IPostRepository>()
            .Setup(x => x.GetPendingPosts())
            .ReturnsAsync(Enumerable.Empty<Post>());

        // Act
        var result = await _queryHandler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeOfType<List<PendingPostResponse>>();
        result.Should().BeEmpty();
    }
}
