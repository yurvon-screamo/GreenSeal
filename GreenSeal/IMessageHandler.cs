namespace GreenSeal;

/// <summary>
/// Event handler
/// </summary>
/// <typeparam name="TMessage">Message type</typeparam>
public interface IMessageHandler<in TMessage> where TMessage : notnull
{
    /// <summary>
    /// Handle event
    /// </summary>
    /// <param name="data">Event body</param>
    /// <param name="ct">Handle cancelation token</param>
    /// <returns></returns>
    ValueTask Handle(TMessage data, CancellationToken ct);
}
