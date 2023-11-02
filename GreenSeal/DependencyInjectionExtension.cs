using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using System.Reflection.Metadata;

namespace GreenSeal;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddMessageHandler<THandler, TMessage>(this IServiceCollection services)
        where THandler : class, IMessageHandler<TMessage>
        where TMessage : notnull
    {
        services.TryAddSingleton<IMessageReceiver, ChannelMessageReceiver<TMessage>>();

        services.AddSingleton<IMessageHandler<TMessage>, THandler>();

        return services;
    }

    public static IServiceCollection AddGreenSeal(this IServiceCollection services)
    {
        services.AddSingleton<IGreenSeal, ChannelGreenSeal>();

        return services;
    }
}
