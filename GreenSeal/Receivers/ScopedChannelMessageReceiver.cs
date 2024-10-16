﻿using GreenSeal.Receivers.Interfaces;

using Microsoft.Extensions.DependencyInjection;

namespace GreenSeal.Receivers;

internal class ScopedChannelMessageReceiver<TMessage> : IMessageReceiver
    where TMessage : notnull
{
    private readonly IServiceProvider _scopeProvider;

    public ScopedChannelMessageReceiver(IServiceProvider scopeProvider)
    {
        _scopeProvider = scopeProvider;
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

        IEnumerable<IMessageHandler<TMessage>> services = _scopeProvider.GetServices<IMessageHandler<TMessage>>();

        foreach (IMessageHandler<TMessage> item in services)
        {
            await item.Handle(message, ct);
        }
    }
}
