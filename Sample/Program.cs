using GreenSeal;
using GreenSeal.Receivers.Interfaces;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddGreenSeal();
builder.Services.AddSingletonMessageHandler<SampleHandler, Ping>();
builder.Services.AddTransientMessageHandler<SampleHandler2, Ping>();

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

IGreenSeal greenSeal = app.Services.GetRequiredService<IGreenSeal>();

await greenSeal.Publish(new Ping(), default);

app.Run();

public readonly record struct Ping;

public class SampleHandler : IMessageHandler<Ping>
{
    public ValueTask Handle(Ping data, CancellationToken ct)
    {
        Console.WriteLine(data.GetType().Name);

        return ValueTask.CompletedTask;
    }
}

public class SampleHandler2 : IMessageHandler<Ping>
{
    public ValueTask Handle(Ping data, CancellationToken ct)
    {
        Console.WriteLine(data.GetType().Name);

        return ValueTask.CompletedTask;
    }
}

public class Example
{
    private readonly IGreenSeal _greenSeal;
    
    public Example(IGreenSeal greenSeal)
    {
        _greenSeal = greenSeal;
    }

    public async Task MyWork(CancellationToken ct)
    {
        /// ... work

         await _greenSeal.Publish(new Ping(), ct);

        /// ... more work
    }
}