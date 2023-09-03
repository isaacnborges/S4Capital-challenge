using System.Security.Claims;

namespace S4Capital.Api.Core;

public abstract class MediatorHandler
{
    private readonly INotificationManager _notificationManager;
    private readonly IHttpContextAccessor _context;

    protected MediatorHandler(INotificationManager notificationManager, IHttpContextAccessor context)
    {
        _notificationManager = notificationManager;
        _context = context;
    }

    protected void Notify(string message) => _notificationManager.PublishNotification(message);

    protected string GetUserId()
    {
        return _context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
