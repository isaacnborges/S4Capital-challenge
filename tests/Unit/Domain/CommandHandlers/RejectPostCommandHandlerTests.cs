using Microsoft.AspNetCore.Http;
using Moq;
using Moq.AutoMock;
using S4Capital.Api.Core;
using S4Capital.Api.Domain.CommandHandlers;
using S4Capital.Api.Domain.Entities;
using S4Capital.Api.Domain.Resources;
using S4Capital.Api.Infrastructure.Repositories;
using S4Capital.Tests.Support;
using S4Capital.Tests.Support.Mocks.Dtos.Commands;
using S4Capital.Tests.Support.Mocks.Entities;
using System.Security.Claims;

namespace S4Capital.Tests.Unit.Domain.CommandHandlers;
public class RejectPostCommandHandlerTests
{
    private readonly AutoMocker _mocker;
    private readonly RejectPostCommandHandler _commandHandler;

    public RejectPostCommandHandlerTests()
    {
        _mocker = new AutoMocker();
        _commandHandler = _mocker.CreateInstance<RejectPostCommandHandler>();
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldRejectPost()
    {
        // Arrange
        var command = RejectPostCommandMock.GetFaker();
        var post = PostMock.GetFaker();

        _mocker.GetMock<IHttpContextAccessor>()
            .Setup(x => x.HttpContext!.User)
            .Returns(new ClaimsPrincipalMock(new Claim(ClaimTypes.NameIdentifier, ClaimsPrincipalMock.DefaultUserId)));

        _mocker.GetMock<IPostRepository>()
            .Setup(x => x.GetPendingByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(post);

        // Act
        await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        _mocker.GetMock<IPostRepository>().Verify(x => x.UpdatePostAsync(post), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenPostNotFound_ShouldNotRejectPostAndNotify()
    {
        // Arrange
        var command = RejectPostCommandMock.GetFaker();

        _mocker.GetMock<IHttpContextAccessor>()
            .Setup(x => x.HttpContext!.User)
            .Returns(new ClaimsPrincipalMock(new Claim(ClaimTypes.NameIdentifier, ClaimsPrincipalMock.DefaultUserId)));

        // Act
        await _commandHandler.Handle(command, CancellationToken.None);

        // Arrange
        _mocker.GetMock<IPostRepository>().Verify(repo => repo.UpdatePostAsync(It.IsAny<Post>()), Times.Never);
        _mocker.GetMock<INotificationManager>().Verify(manager => manager.PublishNotification(Resource.PostNotFound), Times.Once);
    }
}