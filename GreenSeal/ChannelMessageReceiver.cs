using System.Threading.Channels;

namespace GreenSeal;

public class ChannelMessageReceiver<TMessage> : IMessageReceiver<TMessage> where TMessage : notnull
{
    private readonly Channel<TMessage> _channel;
    private readonly Task _runner;

    public ChannelMessageReceiver(CancellationToken ct, params IMessageHandler<TMessage>[] handlers)
    {
        _channel = Channel.CreateUnbounded<TMessage>(new()
        {
            SingleReader = true,
        });

        _runner = Run(_channel.Reader, handlers, ct);
    }

    private static async Task Run(ChannelReader<TMessage> reader, IMessageHandler<TMessage>[] handlers, CancellationToken ct)
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
