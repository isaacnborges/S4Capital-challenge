using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace S4Capital.Tests.Support;
public class UserFilterHelper : IAsyncActionFilter
{
    private readonly List<Claim> _claims;

    public UserFilterHelper(List<Claim> claims)
    {
        _claims = claims;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        context.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(_claims));

        await next();
    }
}
