namespace EventBus;

public interface IMessageHandler<in T> where T : notnull
{
    Task Handle(T data, CancellationToken ct);
}
