using Bogus;
using S4Capital.Api.Api.Dtos.Commands;

namespace S4Capital.Tests.Support.Mocks.Dtos.Commands;
internal static class AddCommentCommandMock
{
    public static AddCommentCommand GetFaker()
    {
        var fake = new Faker<AddCommentCommand>()
            .CustomInstantiator(x => new AddCommentCommand
            {
                PostId = Guid.NewGuid(),
                Comment = x.Random.Words()
            });

        return fake.Generate();
    }
}
