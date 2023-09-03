using MediatR;

namespace S4Capital.Api.Core;

public class NotificationHandler : INotificationHandler<Notification>, IDisposable
{
    private List<Notification> _notifications;

    public NotificationHandler()
    {
        _notifications = new List<Notification>();
    }

    public Task Handle(Notification message, CancellationToken cancellationToken)
    {
        _notifications.Add(message);
        return Task.CompletedTask;
    }

    public virtual List<Notification> GetNotifications()
    {
        return _notifications.Where(not => not.GetType() == typeof(Notification)).ToList();
    }

    public virtual bool HasNotifications()
    {
        return GetNotifications().Any();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        _notifications = new List<Notification>();
    }
}