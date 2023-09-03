using Bogus;
using System.Security.Claims;

namespace S4Capital.Tests.Support;
internal class ClaimsPrincipalMock : ClaimsPrincipal
{
    public static readonly string DefaultUserId = "5ba7bc41-35c9-4c0a-94bd-e9e9cf2fb0af";
    public static readonly string DefaultUserName = new Person().UserName;

    public ClaimsPrincipalMock(params Claim[] claims) : base(new ClaimsIdentityMock(claims))
    { }
}

public class ClaimsIdentityMock : ClaimsIdentity
{
    public ClaimsIdentityMock(params Claim[] claims) : base(claims)
    { }
}
