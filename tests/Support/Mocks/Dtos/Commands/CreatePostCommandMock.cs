using Bogus;
using S4Capital.Api.Api.Dtos.Commands;

namespace S4Capital.Tests.Support.Mocks.Dtos.Commands;
internal static class CreatePostCommandMock
{
    public static CreatePostCommand GetFaker()
    {
        var fake = new Faker<CreatePostCommand>()
            .CustomInstantiator(x => new CreatePostCommand
            {
                Title = x.Name.JobTitle(),
                Content = x.Random.Words()
            });

        return fake.Generate();
    }
}
