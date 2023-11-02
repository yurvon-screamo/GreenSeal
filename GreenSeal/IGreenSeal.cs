namespace GreenSeal;

public interface IGreenSeal : IAsyncDisposable
{
    void Publish<TMessage>(TMessage message) where TMessage : notnull;

    ValueTask StopAsync();
}
