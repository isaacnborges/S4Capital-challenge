using Bogus;
using S4Capital.Api.Api.Dtos.Commands;

namespace S4Capital.Tests.Support.Mocks.Dtos.Commands;
internal static class ApprovePostCommandMock
{
    public static ApprovePostCommand GetFaker()
    {
        var fake = new Faker<ApprovePostCommand>()
            .CustomInstantiator(x => new ApprovePostCommand(x.Random.Guid()));

        return fake.Generate();
    }
}
