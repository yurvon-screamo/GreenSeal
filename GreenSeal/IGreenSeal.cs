namespace GreenSeal;

public interface IGreenSeal
{
    void Publish<TMessage>(TMessage message) where TMessage : notnull;
}
