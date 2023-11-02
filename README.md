# GreenSeal

A simple implementation IPublisher, like in a mediator, is only simpler, dumber and faster

## Benchmarks

* 12th Gen Intel Core i5-1240P, 1 CPU, 16 logical and 12 physical cores
* .NET SDK 7.0.400

| Method                             | Mean      | Error     | StdDev    | Median    | Gen0      | Gen1     | Gen2     | Allocated |
|----------------------------------- |----------:|----------:|----------:|----------:|----------:|---------:|---------:|----------:|
| MediatRPublisher                   | 13.217 ms | 0.0722 ms | 0.0603 ms | 13.213 ms | 6109.3750 |        - |        - |  36.62 MB |
| MediatRPublisherWithEmptyMessage   |  9.119 ms | 0.0415 ms | 0.0388 ms |  9.122 ms | 4078.1250 |        - |        - |  24.41 MB |
| GreenSealPublisher                 | 11.544 ms | 0.2296 ms | 0.3065 ms | 11.516 ms |  640.6250 | 250.0000 | 250.0000 |      4 MB |
| GreenSealPublisherWithEmptyMessage |  9.955 ms | 0.1987 ms | 0.3035 ms |  9.960 ms |  515.6250 |  62.5000 |        - |    3.1 MB |
| NativeTaskInvoker                  |  4.628 ms | 0.0856 ms | 0.2083 ms |  4.557 ms |  953.1250 | 953.1250 | 289.0625 |   1.53 MB |
| NativeTaskInvokerWithEmptyMessage  |  2.747 ms | 0.0526 ms | 0.0563 ms |  2.760 ms |  843.7500 | 843.7500 | 296.8750 |   1.53 MB |
