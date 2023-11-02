using BenchmarkDotNet.Attributes;

using GreenSeal;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace Benchmark;

public readonly record struct Data(string Data1, string Data2) : INotification;

public class HandlerData : IMessageHandler<Data>, INotificationHandler<Data>
{
    public async Task Handle(Data data, CancellationToken ct) => await Task.Delay(0, ct);
}

public class HandlerData2 : IMessageHandler<Data>, INotificationHandler<Data>
{
    public async Task Handle(Data data, CancellationToken ct) => await Task.Delay(0, ct);
}

[MemoryDiagnoser]
[SimpleJob]
public class BenchmarkRunner : BenchmarkEmptyMessage
{
    private const int CountRun = 100000;
    private const string Value = "QWERTY123";

    private readonly ServiceProvider _structs;

    public BenchmarkRunner()
    {
        ServiceCollection collection = new();
        collection.AddSingleton<INotificationHandler<Data>, HandlerData>();
        collection.AddSingleton<INotificationHandler<Data>, HandlerData2>();
        _structs = collection.BuildServiceProvider();
    }

    [Benchmark]
    public async Task NativeTaskInvoker()
    {
        IMessageHandler<Data>[] handlers = new IMessageHandler<Data>[]
        {
            new HandlerData(),
            new HandlerData2()
        };

        Task[] tasks = new Task[CountRun];

        CancellationToken ct = default;

        for (int i = 0; i < CountRun; i++)
        {
            async Task invoke(CancellationToken ct)
            {
                Data data = new(Value, Value);

                foreach (IMessageHandler<Data> handler in handlers)
                {
                    await handler.Handle(data, ct);
                }
            }

            tasks[i] = invoke(ct);
        }

        await Task.WhenAll(tasks);
    }

    [Benchmark]
    public async Task GreenSealPublisher()
    {
        ChannelMessageReceiver<Data> channelsQueue = new(new IMessageHandler<Data>[] { new HandlerData(), new HandlerData2() });

        ChannelGreenSeal greenSeal = new(new IMessageReceiver[]
        {
            channelsQueue,
        });

        for (int i = 0; i < CountRun; i++)
        {
            greenSeal.Publish(new Data(Value, Value));
        }

        await greenSeal.StopAsync();
    }

    [Benchmark]
    public async Task MediatRPublisher()
    {
        Mediator mediator = new(_structs);

        for (int i = 0; i < CountRun; i++)
        {
            await mediator.Publish(new Data(Value, Value));
        }
    }
}
