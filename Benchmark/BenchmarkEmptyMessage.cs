using BenchmarkDotNet.Attributes;

using GreenSeal;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace Benchmark;

public readonly record struct Ping : INotification;

public class EmptyHandler : IMessageHandler<Ping>, INotificationHandler<Ping>
{
    public Task Handle(Ping data, CancellationToken ct) => Task.CompletedTask;
}

[MemoryDiagnoser]
[SimpleJob]
public class BenchmarkEmptyMessage
{
    private const int CountRun = 100000;

    private readonly ServiceProvider _structs;

    public BenchmarkEmptyMessage()
    {
        ServiceCollection collection = new();
        collection.AddSingleton<INotificationHandler<Ping>, EmptyHandler>();
        _structs = collection.BuildServiceProvider();
    }

    [Benchmark]
    public async Task NativeTaskInvokerWithEmptyMessage()
    {
        IMessageHandler<Ping>[] handlers = new IMessageHandler<Ping>[]
        {
            new EmptyHandler(),
        };

        Task[] tasks = new Task[CountRun];

        CancellationToken ct = default;

        for (int i = 0; i < CountRun; i++)
        {
            async Task invoke(CancellationToken ct)
            {
                Ping data = new();

                foreach (IMessageHandler<Ping> handler in handlers)
                {
                    await handler.Handle(data, ct);
                }
            }

            tasks[i] = invoke(ct);
        }

        await Task.WhenAll(tasks);
    }

    [Benchmark]
    public async Task GreenSealPublisherWithEmptyMessage()
    {
        ChannelMessageReceiver<Ping> channelsQueue = new(default, new EmptyHandler());

        for (int i = 0; i < CountRun; i++)
        {
            channelsQueue.Publish(new());
        }

        await channelsQueue.StopAsync();
    }

    [Benchmark]
    public async Task MediatRPublisherWithEmptyMessage()
    {
        Mediator mediator = new(_structs);

        for (int i = 0; i < CountRun; i++)
        {
            await mediator.Publish(new Ping());
        }
    }
}
