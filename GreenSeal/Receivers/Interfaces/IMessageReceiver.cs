namespace GreenSeal.Receivers.Interfaces;

internal interface IMessageReceiver
{
    Type GetReceiverType();
    ValueTask PublishAsync(object data, CancellationToken ct);
}
