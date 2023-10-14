# GreenSeal

A simple implementation IPublisher, like in a mediator, is only simpler, dumber and faster

## Benchmarks

```
BenchmarkDotNet v0.13.9+228a464e8be6c580ad9408e98f18813f6407fb5a, Windows 11 (10.0.22621.2134/22H2/2022Update/SunValley2)
12th Gen Intel Core i5-1240P, 1 CPU, 16 logical and 12 physical cores
.NET SDK 7.0.400
  [Host]     : .NET 7.0.10 (7.0.1023.36312), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.10 (7.0.1023.36312), X64 RyuJIT AVX2
```

| Method           | Mean      | Error     | StdDev    | Gen0      | Gen1     | Gen2     | Allocated |
|----------------- |----------:|----------:|----------:|----------:|---------:|---------:|----------:|
| SimpleInvoker    |  4.618 ms | 0.0538 ms | 0.0503 ms |  492.1875 | 492.1875 | 492.1875 |   1.53 MB |
| ChannelsReceiver |  9.498 ms | 0.1822 ms | 0.2891 ms |  281.2500 | 281.2500 | 281.2500 |   1.11 MB |
| MediatorReceiver | 13.670 ms | 0.1871 ms | 0.1563 ms | 6109.3750 |        - |        - |  36.62 MB |

