using GreenSeal.Receivers;
using GreenSeal.Receivers.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
    /// <typeparam name="THandler">Handler type</typeparam>
    /// <typeparam name="TMessage">Message type</typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddTransientMessageHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TMessage>(this IServiceCollection services)
        where THandler : class, IMessageHandler<TMessage>
        where TMessage : notnull
    {
        services.TryAddSingleton<IMessageReceiver, TransientChannelMessageReceiver<TMessage>>();

        services.AddTransient<IMessageHandler<TMessage>, THandler>();

        return services;
    }

    /// <summary>
    /// Add event handler with singleton life time
    /// </summary>
    /// <typeparam name="THandler">Handler type</typeparam>
    /// <typeparam name="TMessage">Message type</typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddSingletonMessageHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TMessage>(this IServiceCollection services)
        where THandler : class, IMessageHandler<TMessage>
        where TMessage : notnull
    {
        services.TryAddSingleton<IMessageReceiver, SingletonChannelMessageReceiver<TMessage>>();

        services.AddSingleton<IMessageHandler<TMessage>, THandler>();

        return services;
    }

    /// <summary>
    /// Add IGreenSeal implementation
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddGreenSeal(this IServiceCollection services)
    {
        services.AddSingleton<IGreenSeal, Receivers.GreenSeal>();

        return services;
    }
}
