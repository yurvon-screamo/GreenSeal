using System.Collections.Frozen;

using GreenSeal.Receivers.Interfaces;

namespace GreenSeal.Receivers;

internal class GreenSeal : IGreenSeal
{
    private readonly FrozenDictionary<Type, IMessageReceiver> _receiversMap;

    public GreenSeal(IEnumerable<IMessageReceiver> receivers)
    {
        _receiversMap = receivers.ToFrozenDictionary(c => c.GetReceiverType());
    }

    public ValueTask Publish<TMessage>(TMessage message, CancellationToken ct) where TMessage : notnull
    {
        IMessageReceiver receiver = _receiversMap[message.GetType()];

        return receiver.PublishAsync(message, ct);
    }
}
