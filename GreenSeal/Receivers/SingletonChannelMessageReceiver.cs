using GreenSeal.Receivers.Interfaces;

namespace GreenSeal.Receivers;

internal class SingletonChannelMessageReceiver<TMessage> : IMessageReceiver
    where TMessage : notnull
{
    private readonly IEnumerable<IMessageHandler<TMessage>> _handlers;

    public SingletonChannelMessageReceiver(IEnumerable<IMessageHandler<TMessage>> handlers)
    {
        _handlers = handlers;
    }

    public Type GetReceiverType()
    {
        return typeof(TMessage);
    }

    public async ValueTask PublishAsync(object data, CancellationToken ct)
    {
        if (data is not TMessage message)
        {
            return;
        }

        foreach (IMessageHandler<TMessage> handler in _handlers)
        {
            await handler.Handle(message, ct);
        }
    }
}
