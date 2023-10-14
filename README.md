# GreenSeal

A simple implementation IPublisher, like in a mediator, is only simpler, dumber and faster

## Benchmarks

* 12th Gen Intel Core i5-1240P, 1 CPU, 16 logical and 12 physical cores
* .NET SDK 7.0.400

| Method                             | Mean      | Error     | StdDev    | Gen0      | Gen1     | Gen2     | Allocated   |
|----------------------------------- |----------:|----------:|----------:|----------:|---------:|---------:|------------:|
| NativeTaskInvoker                  |  4.757 ms | 0.0925 ms | 0.1028 ms |  492.1875 | 492.1875 | 492.1875 |   1562.9 KB |
| GreenSealPublisher                 |  9.641 ms | 0.1568 ms | 0.1390 ms |  250.0000 | 250.0000 | 250.0000 |   980.58 KB |
| MediatRPublisher                   | 13.614 ms | 0.1011 ms | 0.0844 ms | 6109.3750 |        - |        - | 37500.06 KB |
| NativeTaskInvoker WithEmptyMessage  |  2.410 ms | 0.0440 ms | 0.0411 ms |  496.0938 | 496.0938 | 496.0938 |  1562.87 KB |
| GreenSealPublisher WithEmptyMessage |  8.806 ms | 0.1740 ms | 0.3048 ms |         - |        - |        - |     60.1 KB |
| MediatRPublisher WithEmptyMessage   |  9.588 ms | 0.1808 ms | 0.1691 ms | 4078.1250 |        - |        - | 25000.06 KB |
