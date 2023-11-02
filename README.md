# GreenSeal

A simple implementation IPublisher, like in a mediator, is only simpler, dumber and faster

## Benchmarks

* 12th Gen Intel Core i5-1240P, 1 CPU, 16 logical and 12 physical cores
* .NET SDK 7.0.400

| Method                             |      Mean | Allocated |
| ---------------------------------- | --------: | --------: |
| MediatRPublisher                   | 13.217 ms |  36.62 MB |
| MediatRPublisherWithEmptyMessage   |  9.119 ms |  24.41 MB |
| GreenSealPublisher                 | 11.544 ms |      4 MB |
| GreenSealPublisherWithEmptyMessage |  9.955 ms |    3.1 MB |
| NativeTaskInvoker                  |  4.628 ms |   1.53 MB |
| NativeTaskInvokerWithEmptyMessage  |  2.747 ms |   1.53 MB |
