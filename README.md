# GreenSeal

A simple implementation IPublisher, like in a mediator, is only simpler, dumber, faster and AOT ready.

## Usage

1. Install:

```csharp
    <PackageReference Include="GreenSeal" Version="0.0.1" />
```

2. Add to DI IGreenSeal publisher:

```csharp
builder.Services.AddGreenSeal();
builder.Services.AddSingletonMessageHandler<SampleHandler, Ping>();
```

3. Define event body:

```csharp
public readonly record struct Ping;
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

public class SampleHandler3 : IMessageHandler<Ping> ...

```

5. Add handler(s) to DI:

```csharp
builder.Services.AddSingletonMessageHandler<SampleHandler, Ping>();
builder.Services.AddTransientMessageHandler<SampleHandler2, Ping>();
builder.Services.AddTransientMessageHandler<SampleHandler3, Ping>();
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
