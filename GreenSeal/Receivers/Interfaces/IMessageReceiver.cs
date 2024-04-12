namespace GreenSeal.Receivers.Interfaces;

internal interface IMessageReceiver
{
    Type GetReceiverType();
}

internal interface IMessageReceiver<in TMessage> : IMessageReceiver
    where TMessage : notnull
{
    ValueTask PublishAsync(TMessage data, CancellationToken ct);
}
