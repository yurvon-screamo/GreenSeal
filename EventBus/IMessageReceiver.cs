namespace EventBus;

public interface IMessageReceiver<in TMessage> where TMessage : notnull
{
    void Publish(TMessage data);

    ValueTask StopAsync();
}
