using Bogus;
using S4Capital.Api.Api.Dtos.Commands;

namespace S4Capital.Tests.Support.Mocks.Dtos.Commands;
internal static class SubmitPostCommandMock
{
    public static SubmitPostCommand GetFaker()
    {
        var fake = new Faker<SubmitPostCommand>()
            .CustomInstantiator(x => new SubmitPostCommand(x.Random.Guid()));

        return fake.Generate();
    }
}