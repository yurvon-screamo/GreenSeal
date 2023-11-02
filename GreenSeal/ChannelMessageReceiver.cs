using System.Threading.Channels;

namespace GreenSeal;

public class ChannelMessageReceiver<TMessage> : IMessageReceiver<TMessage> where TMessage : notnull
{
    private readonly Channel<TMessage> _channel;
    private readonly Task _runner;

    public ChannelMessageReceiver(IEnumerable<IMessageHandler<TMessage>> handlers)
    {
        _channel = Channel.CreateUnbounded<TMessage>(new()
        {
            SingleReader = true,
        });

        //TODO: add ct
        _runner = Run(_channel.Reader, handlers, default);
    }

    private static async Task Run(ChannelReader<TMessage> reader, IEnumerable<IMessageHandler<TMessage>> handlers, CancellationToken ct)
    {
        await foreach (TMessage data in reader.ReadAllAsync(ct))
        {
            foreach (IMessageHandler<TMessage> h in handlers)
            {
                await h.Handle(data, ct);
            }
        }
    }

    public Type GetReceiverType()
    {
        return typeof(TMessage);
    }

    public void Publish(TMessage data)
    {
        _channel.Writer.TryWrite(data);
    }

    public void Publish(object data)
    {
        Publish((TMessage)data);
    }

    public async ValueTask StopAsync()
    {
        _channel.Writer.Complete();
        await _runner;
    }
}
