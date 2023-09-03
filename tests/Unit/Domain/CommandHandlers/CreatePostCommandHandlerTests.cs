using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Moq.AutoMock;
using S4Capital.Api.Domain.CommandHandlers;
using S4Capital.Api.Domain.Entities;
using S4Capital.Api.Extensions;
using S4Capital.Api.Infrastructure.Repositories;
using S4Capital.Tests.Support;
using S4Capital.Tests.Support.Mocks.Dtos.Commands;
using System.Security.Claims;

namespace S4Capital.Tests.Unit.Domain.CommandHandlers;
public class CreatePostCommandHandlerTests
{
    private readonly AutoMocker _mocker;
    private readonly CreatePostCommandHandler _commandHandler;

    public CreatePostCommandHandlerTests()
    {
        _mocker = new AutoMocker();
        _commandHandler = _mocker.CreateInstance<CreatePostCommandHandler>();
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldCreatePost()
    {
        // Arrange
        var command = CreatePostCommandMock.GetFaker();

        _mocker.GetMock<IHttpContextAccessor>()
            .Setup(x => x.HttpContext!.User)
            .Returns(new ClaimsPrincipalMock(new Claim(ClaimTypes.NameIdentifier, ClaimsPrincipalMock.DefaultUserId)));

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        _mocker.GetMock<IPostRepository>().Verify(repo => repo.CreatePostAsync(It.IsAny<Post>()), Times.Once);

        result.Id.Should().NotBeEmpty();
        result.Title.Should().Be(command.Title);
        result.Content.Should().Be(command.Content);
        result.Status.Should().Be(PostStatus.Pending.GetDescription());
        result.CreatedBy.Should().Be(ClaimsPrincipalMock.DefaultUserId);
    }
}
