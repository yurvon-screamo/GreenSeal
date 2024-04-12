namespace GreenSeal.Receivers.Interfaces;

/// <summary>
/// A single interface for sending events
/// </summary>
public interface IGreenSeal
{
    /// <summary>
    /// Publish event
    /// </summary>
    /// <typeparam name="TMessage">Event type</typeparam>
    /// <param name="message">Event body</param>
    /// <param name="ct">Handler cancel token</param>
    /// <returns></returns>
    ValueTask Publish<TMessage>(TMessage message, CancellationToken ct) where TMessage : notnull;
}
