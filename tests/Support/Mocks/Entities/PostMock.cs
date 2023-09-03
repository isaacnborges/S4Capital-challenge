using Bogus;
using Microsoft.AspNetCore.Identity;
using S4Capital.Api.Domain.Entities;
using S4Capital.Tests.Support.Mocks.Dtos.Commands;

namespace S4Capital.Tests.Support.Mocks.Entities;
internal static class PostMock
{
    public static List<Post> GetListFaker(int quantity = 1)
    {
        var fake = new Faker<Post>()
            .CustomInstantiator(x => new Post(CreatePostCommandMock.GetFaker(), ClaimsPrincipalMock.DefaultUserId)
            {
                Author = new IdentityUser
                {
                    UserName = ClaimsPrincipalMock.DefaultUserName
                }
            });

        return fake.Generate(quantity);
    }

    public static Post GetFaker()
    {
        return GetListFaker().FirstOrDefault()!;
    }

    public static List<Post> GetListFakerPublished(int quantity = 1)
    {
        var fake = new Faker<Post>()
            .CustomInstantiator(x => new Post(CreatePostCommandMock.GetFaker(), ClaimsPrincipalMock.DefaultUserId)
            {
                Author = new IdentityUser
                {
                    UserName = ClaimsPrincipalMock.DefaultUserName
                }
            });

        var posts = fake.Generate(quantity);

        foreach (var post in posts)
        {
            post.Status = PostStatus.Published;
        }

        return posts;
    }

    public static Post GetFakerPublished()
    {
        return GetListFakerPublished().FirstOrDefault()!;
    }
}
