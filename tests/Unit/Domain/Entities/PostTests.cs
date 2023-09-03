using FluentAssertions;
using S4Capital.Api.Domain.Entities;
using S4Capital.Tests.Support.Mocks.Dtos.Commands;
using S4Capital.Tests.Support.Mocks.Entities;

namespace S4Capital.Tests.Unit.Domain.Entities;
public class PostTests
{
    private readonly string _userId;

    public PostTests()
    {
        _userId = Guid.NewGuid().ToString();
    }

    [Fact]
    public void Update_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var post = PostMock.GetFaker();
        var command = EditPostCommandMock.GetFaker();
        var expectedEditedDate = DateTime.UtcNow;

        // Act
        post.Update(command, _userId);

        // Assert
        post.Title.Should().Be(command.Title);
        post.Content.Should().Be(command.Content);
        post.EditedBy.Should().Be(_userId);
        post.EditedDate.Should().BeCloseTo(expectedEditedDate, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Approve_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var post = PostMock.GetFaker();
        var expectedApprovedDate = DateTime.UtcNow;

        // Act
        post.Approve(_userId);

        // Assert
        post.Status.Should().Be(PostStatus.Approved);
        post.ApprovedBy.Should().Be(_userId);
        post.ApprovedDate.Should().BeCloseTo(expectedApprovedDate, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Reject_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var post = PostMock.GetFaker();
        var command = RejectPostCommandMock.GetFaker();
        var expectedRejectDate = DateTime.UtcNow;

        // Act
        post.Reject(command, _userId);

        // Assert
        post.Status.Should().Be(PostStatus.Rejected);
        post.RejectBy.Should().Be(_userId);
        post.RejectDate.Should().BeCloseTo(expectedRejectDate, TimeSpan.FromSeconds(1));
        post.RejectionComment.Should().Be(command.RejectionComment);
    }

    [Fact]
    public void Reject_CommentIfEmpty_ShouldNotSetRejection()
    {
        // Arrange
        var post = PostMock.GetFaker();
        var command = RejectPostCommandMock.GetFaker();
        command.RejectionComment = string.Empty;

        // Act
        post.Reject(command, _userId);

        // Assert
        post.Status.Should().Be(PostStatus.Rejected);
        post.RejectionComment.Should().BeNullOrEmpty();
    }

    [Fact]
    public void Publish_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var post = PostMock.GetFaker();
        var expectedSubmittedDate = DateTime.UtcNow;

        // Act
        post.Publish(_userId);

        // Assert
        post.Status.Should().Be(PostStatus.Published);
        post.SubmittedBy.Should().Be(_userId);
        post.SubmittedDate.Should().BeCloseTo(expectedSubmittedDate, TimeSpan.FromSeconds(1));
    }
}