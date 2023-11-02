using System.Collections.Immutable;

namespace GreenSeal;

public class ChannelGreenSeal : IGreenSeal
{
    private readonly ImmutableDictionary<Type, IMessageReceiver> _receiversMap;

    public ChannelGreenSeal(IEnumerable<IMessageReceiver> receivers)
    {
        _receiversMap = receivers.ToImmutableDictionary(
            c => c.GetReceiverType(),
            c => c);
    }

    public void Publish<TMessage>(TMessage message) where TMessage : notnull
    {
        IMessageReceiver<TMessage> receiver = (IMessageReceiver<TMessage>)_receiversMap[typeof(TMessage)];

        receiver.Publish(message);
    }

    public async ValueTask StopAsync()
    {
        foreach (KeyValuePair<Type, IMessageReceiver> item in _receiversMap)
        {
            await item.Value.StopAsync();
        }
    }

    public ValueTask DisposeAsync()
    {
        return StopAsync();
    }
}
