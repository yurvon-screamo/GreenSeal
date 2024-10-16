using GreenSeal.Receivers.Interfaces;

namespace GreenSeal.Receivers;

internal class SingletonChannelMessageReceiver<TMessage> : IMessageReceiver<TMessage>
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

    public async ValueTask PublishAsync(TMessage data, CancellationToken ct)
    {
        foreach (IMessageHandler<TMessage> handler in _handlers)
        {
            await handler.Handle(data, ct);
        }
    }
}
