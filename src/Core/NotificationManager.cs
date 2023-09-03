using MediatR;

namespace S4Capital.Api.Core;

public class NotificationManager : INotificationManager
{
    private readonly NotificationHandler _messageHandler;

    public NotificationManager(INotificationHandler<Notification> notification)
    {
        _messageHandler = (NotificationHandler)notification;
    }

    public NotificationManager Invoke()
    {
        return this;
    }

    public bool IsValid()
    {
        return !_messageHandler.HasNotifications();
    }

    public void PublishNotification(string message)
    {
        _messageHandler.Handle(new Notification(message), default);
    }
}