using GreenSeal.Receivers;
using GreenSeal.Receivers.Interfaces;

using Microsoft.Extensions.DependencyInjection;

using System.Diagnostics.CodeAnalysis;

namespace GreenSeal;

/// <summary>
/// Di extend
/// </summary>
public static class DependencyInjectionExtension
{
    /// <summary>
    /// Add event handler with transient life time
    /// </summary>
    public static IServiceCollection AddTransientMessageHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TMessage>(this IServiceCollection services) where THandler : class, IMessageHandler<TMessage> where TMessage : notnull
    {
        services.AddTransient<IMessageHandler<TMessage>, THandler>();
        return services;
    }

    /// <summary>
    /// Add event handler with scoped life time
    /// </summary>
    public static IServiceCollection AddScopedMessageHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TMessage>(this IServiceCollection services) where THandler : class, IMessageHandler<TMessage> where TMessage : notnull
    {
        services.AddScoped<IMessageHandler<TMessage>, THandler>();

        return services;
    }

    /// <summary>
    /// Add event handler with singleton life time
    /// </summary>
    public static IServiceCollection AddSingletonMessageHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TMessage>(this IServiceCollection services)
        where THandler : class, IMessageHandler<TMessage>
        where TMessage : notnull
    {
        services.AddSingleton<IMessageHandler<TMessage>, THandler>();

        return services;
    }

    /// <summary>
    /// Register event to green seal
    /// </summary>
    public static IServiceCollection AddEventReceiver<TMessage>(this IServiceCollection services) where TMessage : notnull
    {
        services.AddSingleton<IMessageReceiver, TransientChannelMessageReceiver<TMessage>>();

        return services;
    }

    /// <summary>
    /// Register event to green seal as scoped
    /// </summary>
    public static IServiceCollection AddScopedEventReceiver<TMessage>(this IServiceCollection services) where TMessage : notnull
    {
        services.AddScoped<IMessageReceiver, ScopedChannelMessageReceiver<TMessage>>();
        return services;
    }

    /// <summary>
    /// Register event as singleton subscriber (only Singleton handler) to green seal
    /// </summary>
    public static IServiceCollection AddSingletonEventReceiver<TMessage>(this IServiceCollection services)
        where TMessage : notnull
    {
        services.AddSingleton<IMessageReceiver, SingletonChannelMessageReceiver<TMessage>>();

        return services;
    }

    /// <summary>
    /// Add IGreenSeal implementation
    /// </summary>
    public static IServiceCollection AddGreenSeal(this IServiceCollection services)
    {
        services.AddSingleton<IGreenSeal, Receivers.GreenSeal>();

        return services;
    }

    /// <summary>
    /// Add IGreenSeal implementation as scoped
    /// </summary>
    public static IServiceCollection AddScopedGreenSeal(this IServiceCollection services)
    {
        services.AddScoped<IGreenSeal, Receivers.GreenSeal>();

        return services;
    }
}
