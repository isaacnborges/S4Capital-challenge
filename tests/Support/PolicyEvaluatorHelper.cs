using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace S4Capital.Tests.Support;
public class PolicyEvaluatorHelper : IPolicyEvaluator
{
    private readonly List<Claim> _claims;

    public PolicyEvaluatorHelper(List<Claim> claims)
    {
        _claims = claims;
    }

    public virtual async Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
    {
        var claimsPrincipal = new ClaimsPrincipal();
        claimsPrincipal.AddIdentity(new ClaimsIdentity(_claims, "authenticationType"));

        return await Task.FromResult(AuthenticateResult.Success(
            new AuthenticationTicket(claimsPrincipal, new AuthenticationProperties(), "authenticationScheme")));
    }

    public virtual async Task<PolicyAuthorizationResult> AuthorizeAsync(
        AuthorizationPolicy policy, AuthenticateResult authenticationResult, HttpContext context, object resource)
    {
        return await Task.FromResult(PolicyAuthorizationResult.Success());
    }
}