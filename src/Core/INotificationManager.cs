namespace S4Capital.Api.Core;

public interface INotificationManager
{
    bool IsValid();
    void PublishNotification(string message);
}
