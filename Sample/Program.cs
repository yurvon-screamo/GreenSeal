using GreenSeal;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddGreenSeal();
builder.Services.AddMessageHandler<SampleHandler, Ping>();

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

IGreenSeal greenSeal = app.Services.GetRequiredService<IGreenSeal>();

greenSeal.Publish(new Ping());

app.Run();

public readonly record struct Ping;

public class SampleHandler : IMessageHandler<Ping>
{
    public Task Handle(Ping data, CancellationToken ct)
    {
        Console.WriteLine(data.GetType().Name);

        return Task.CompletedTask;
    }
}