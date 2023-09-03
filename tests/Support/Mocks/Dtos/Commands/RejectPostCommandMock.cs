using Bogus;
using S4Capital.Api.Api.Dtos.Commands;

namespace S4Capital.Tests.Support.Mocks.Dtos.Commands;
internal static class RejectPostCommandMock
{
    public static RejectPostCommand GetFaker()
    {
        var fake = new Faker<RejectPostCommand>()
            .CustomInstantiator(x => new RejectPostCommand
            {
                PostId = Guid.NewGuid(),
                RejectionComment = x.Random.Words()
            });

        return fake.Generate();
    }
}