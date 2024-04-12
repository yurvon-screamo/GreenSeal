using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using GreenSeal.Receivers.Interfaces;
using MediatR;

using MessagePipe;

using Microsoft.Extensions.DependencyInjection;

namespace Mediator.Benchmarks.Notification;

public sealed record SomeNotification(Guid Id) : INotification;

public sealed class SomeHandlerClass
    : INotificationHandler<SomeNotification>,
      IAsyncMessageHandler<SomeNotification>,
      GreenSeal.Receivers.Interfaces.IMessageHandler<SomeNotification>
{
    public Task Handle(SomeNotification data, CancellationToken ct) => Task.CompletedTask;

    public ValueTask HandleAsync(SomeNotification message, CancellationToken cancellationToken) => default;

    Task INotificationHandler<SomeNotification>.Handle(
        SomeNotification notification,
        CancellationToken cancellationToken
    ) => Task.CompletedTask;
}

[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
[MemoryDiagnoser]
[SimpleJob]
//[EventPipeProfiler(EventPipeProfile.CpuSampling)]
//[DisassemblyDiagnoser]
//[InliningDiagnoser(logFailuresOnly: true, allowedNamespaces: new[] { "Mediator" })]
public class NotificationBenchmarks
{
    private IServiceProvider _serviceProvider;
    private IServiceScope _serviceScope;

    private IGreenSeal _greenSeal;
    private IMediator _mediatr;
    private IAsyncSubscriber<SomeNotification> _messagePipeSubscriber;
    private IAsyncPublisher<SomeNotification> _messagePipePublisher;

    private IDisposable _subscription;

    private SomeHandlerClass _handler;
    private SomeNotification _notification;

    [GlobalSetup]
    public void Setup()
    {
        ServiceCollection services = new();

        services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(SomeHandlerClass).Assembly));
        services.AddMessagePipe(opts => opts.InstanceLifetime = InstanceLifetime.Singleton);
        services.AddSingleton<SomeHandlerClass>();
        services.AddSingleton<INotificationHandler<SomeNotification>, SomeHandlerClass>();

        _serviceProvider = services.BuildServiceProvider();

        _greenSeal= new ChannelGreenSeal(new IMessageReceiver[]
        {
            new ChannelMessageReceiver<SomeNotification>(new GreenSeal.Receivers.Interfaces.IMessageHandler<SomeNotification>[] { new SomeHandlerClass() })
        });

        _mediatr = _serviceProvider.GetRequiredService<IMediator>();
        _handler = _serviceProvider.GetRequiredService<SomeHandlerClass>();

        _messagePipeSubscriber = _serviceProvider.GetRequiredService<IAsyncSubscriber<SomeNotification>>();
        _messagePipePublisher = _serviceProvider.GetRequiredService<IAsyncPublisher<SomeNotification>>();
        
        _subscription = _messagePipeSubscriber.Subscribe(_handler);
        _notification = new(Guid.NewGuid());
    }

    [GlobalCleanup]
    public async ValueTask Cleanup()
    {
        _subscription?.Dispose();
        if (_serviceScope is not null)
            _serviceScope.Dispose();
        else
            (_serviceProvider as IDisposable)?.Dispose();

        await _greenSeal.StopAsync();
    }

    [Benchmark]
    public Task SendNotification_MediatR()
    {
        return _mediatr.Publish(_notification, CancellationToken.None);
    }

    [Benchmark]
    public ValueTask SendNotification_MessagePipe()
    {
        return _messagePipePublisher.PublishAsync(_notification, CancellationToken.None);
    }

    [Benchmark]
    public void SendNotification_GreenSeal()
    {
        _greenSeal.Publish(_notification);
    }

    [Benchmark(Baseline = true)]
    public ValueTask SendNotification_Baseline()
    {
        return _handler.HandleAsync(_notification, CancellationToken.None);
    }
}