using GreenSeal.Receivers.Interfaces;

using Microsoft.Extensions.DependencyInjection;

namespace GreenSeal.Receivers;

internal class TransientChannelMessageReceiver<TMessage> : IMessageReceiver
    where TMessage : notnull
{
    private readonly IServiceScopeFactory _scopeFactory;

    public TransientChannelMessageReceiver(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public Type GetReceiverType()
    {
        return typeof(TMessage);
    }

    public async ValueTask PublishAsync(object data, CancellationToken ct)
    {
        if (data is not TMessage message)
        {
            return;
        }

        using IServiceScope scope = _scopeFactory.CreateScope();

        IEnumerable<IMessageHandler<TMessage>> handlers = scope.ServiceProvider
            .GetServices<IMessageHandler<TMessage>>();

        foreach (IMessageHandler<TMessage> handler in handlers)
        {
            await handler.Handle(message, ct);
        }
    }
}
