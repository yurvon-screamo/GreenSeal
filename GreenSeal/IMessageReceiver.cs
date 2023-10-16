namespace GreenSeal;

public interface IMessageReceiver
{
    Type GetReceiverType();

    void Publish(object data);

    ValueTask StopAsync();
}

public interface IMessageReceiver<in TMessage> : IMessageReceiver where TMessage : notnull
{
    void Publish(TMessage data);
}
