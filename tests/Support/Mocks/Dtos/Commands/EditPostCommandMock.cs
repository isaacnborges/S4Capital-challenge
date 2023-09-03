using Bogus;
using S4Capital.Api.Api.Dtos.Commands;

namespace S4Capital.Tests.Support.Mocks.Dtos.Commands;
internal static class EditPostCommandMock
{
    public static EditPostCommand GetFaker()
    {
        var fake = new Faker<EditPostCommand>()
            .CustomInstantiator(x => new EditPostCommand
            {
                PostId = Guid.NewGuid(),
                Title = x.Name.JobTitle(),
                Content = x.Random.Words()
            });

        return fake.Generate();
    }
}
