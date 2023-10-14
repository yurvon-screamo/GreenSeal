namespace GreenSeal;

public interface IMessageHandler<in T> where T : notnull
{
    Task Handle(T data, CancellationToken ct);
}
