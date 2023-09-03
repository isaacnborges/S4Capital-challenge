using MediatR;

namespace S4Capital.Api.Core;

public class Notification : INotification
{
    public string Value { get; }

    public Notification(string value)
    {
        Value = value;
    }
}