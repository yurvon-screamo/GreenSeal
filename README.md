# GreenSeal

A simple implementation IPublisher, like in a mediator, is only simpler, dumber and faster

## Benchmarks

* 12th Gen Intel Core i5-1240P, 1 CPU, 16 logical and 12 physical cores
* .NET SDK 7.0.400

| Method                              | Mean      | Allocated |
|------------------------------------ |----------:|----------:|
| GreenSealPublisher WithEmptyMessage |  8.8 ms   | 60.1 KB   |
| GreenSealPublisher                  |  9.6 ms   | 980.5 KB  |
| NativeTaskInvoker                   |  4.7 ms   | 1562.9 KB |
| NativeTaskInvoker WithEmptyMessage  |  2.4 ms   | 1562.9 KB |
| MediatRPublisher                    | 13.6 ms   | 37500 KB  |
| MediatRPublisher WithEmptyMessage   |  9.5 ms   | 25000 KB  |
