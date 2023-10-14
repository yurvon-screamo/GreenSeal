using System.Threading.Channels;

namespace EventBus;

public class ChannelMessageReceiver<T> : IMessageReceiver<T> where T : notnull
{
    private readonly Channel<T> _channel;
    private readonly Task _runner;

    public ChannelMessageReceiver(CancellationToken ct, params IMessageHandler<T>[] handlers)
    {
        _channel = Channel.CreateUnbounded<T>(new()
        {
            SingleReader = true,
        });

        _runner = Run(_channel.Reader, ct, handlers);
    }

    private static async Task Run(ChannelReader<T> reader, CancellationToken ct, IMessageHandler<T>[] handlers)
    {
        await foreach (T data in reader.ReadAllAsync().WithCancellation(ct))
        {
            foreach (IMessageHandler<T> h in handlers)
            {
                await h.Handle(data, ct);
            }
        }
    }

    public void Publish(T data)
    {
        _channel.Writer.TryWrite(data);
    }

    public async ValueTask StopAsync()
    {
        _channel.Writer.Complete();
        await _runner;
    }
}
