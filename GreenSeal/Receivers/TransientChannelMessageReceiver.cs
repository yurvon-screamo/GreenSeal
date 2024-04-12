using GreenSeal.Receivers.Interfaces;

using Microsoft.Extensions.DependencyInjection;

namespace GreenSeal.Receivers;

internal class TransientChannelMessageReceiver<TMessage> : IMessageReceiver<TMessage> where TMessage : notnull
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IEnumerable<IMessageHandler<TMessage>> _handlers;

    public TransientChannelMessageReceiver(IServiceScopeFactory scopeFactory, IEnumerable<IMessageHandler<TMessage>> handlers)
    {
        _scopeFactory = scopeFactory;
        _handlers = handlers;
    }

    public Type GetReceiverType()
    {
        return typeof(TMessage);
    }

    public async ValueTask PublishAsync(TMessage data, CancellationToken ct)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        IEnumerable<IMessageHandler<TMessage>> handlers = scope.ServiceProvider
            .GetServices<IMessageHandler<TMessage>>();

        foreach (IMessageHandler<TMessage> handler in handlers)
        {
            await handler.Handle(data, ct);
        }
    }
}
