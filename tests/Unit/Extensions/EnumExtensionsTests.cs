using FluentAssertions;
using S4Capital.Api.Core.Extensions;
using S4Capital.Api.Domain.Entities;

namespace S4Capital.Tests.Unit.Extensions;
public class EnumExtensionsTests
{
    [Theory]
    [InlineData(PostStatus.Pending, "Pending")]
    [InlineData(PostStatus.Approved, "Approved")]
    [InlineData(PostStatus.Published, "Published")]
    [InlineData(PostStatus.Rejected, "Rejected")]
    public void GetDescription_ShouldReturnCorrectDescription(PostStatus status, string expectedDescription)
    {
        // Act
        var description = status.GetDescription();

        // Assert
        description.Should().Be(expectedDescription);
    }
}
