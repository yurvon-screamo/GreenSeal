# GreenSeal

A simple implementation IPublisher, like in a mediator, is only simpler, dumber, faster and AOT ready.

## Usage

1. Install:

```csharp
    <PackageReference Include="GreenSeal" Version="0.0.2" />
```

2. Add to DI IGreenSeal publisher:

```csharp
builder.Services.AddGreenSeal();
```

3. Define event body:

```csharp
public readonly record struct Ping;
public readonly record struct SingleBar;
```

4. Implement event handler(s):

```csharp

public class SampleHandler : IMessageHandler<Ping>
{
    public ValueTask Handle(Ping data, CancellationToken ct)
    {
        Console.WriteLine(data.GetType().Name);

        return ValueTask.CompletedTask;
    }
}


public class SampleHandler2 : IMessageHandler<Ping> ...

public class SampleHandler3 : IMessageHandler<SingleBar> ...

```

5. Add handlers and receivers to DI. Use `AddSingletonEventReceiver` if all handlers has singleton lifetime, other use `AddEventReceiver`.

```csharp
builder.Services.AddEventReceiver<Ping>();
builder.Services.AddSingletonMessageHandler<SampleHandler, Ping>();
builder.Services.AddTransientMessageHandler<SampleHandler2, Ping>();

builder.Services.AddSingletonEventReceiver<SingleBar>();
builder.Services.AddSingletonMessageHandler<SampleHandler3, SingleBar>();
```

6. Use!

```csharp
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
```
