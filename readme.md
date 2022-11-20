# ASP.NET Core 6 Performance

This is the repository with the code associated with an upcoming Pluralsight course called "ASP.NET Core 6 Performance".

Repo URL: [https://github.com/dahlsailrunner/aspnet6-performance](https://github.com/dahlsailrunner/aspnet6-performance)

## VS Code Setup

The `C#` extension is required to use this repo.  I have some other settings that you may be curious about
and they are described in my [VS Code gist](https://gist.github.com/dahlsailrunner/1765b807940e29951ea6bdfb36cd85dd).

## Redis

The [Docker image for Redis](https://hub.docker.com/_/redis/) is used as a distributed cache in this code.  To set
it up, use the following Docker CLI commands:

```bash
docker pull redis
docker run -d --name redisDev -p 6379:6379 redis
```

The NuGet package used is [Microsoft.Extensions.Caching.StackExchangeRedis](https://www.nuget.org/packages/Microsoft.Extensions.Caching.StackExchangeRedis)

## Seq

The Docker image for Seq is used for logging and instrumention in this code as well.  Use the following commands:

```bash
docker pull datalust/seq
docker run -d --name seq --restart unless-stopped -e ACCEPT_EULA=Y -p 5341:80 datalust/seq
```

The NuGet package used for this is [Serilog.Sinks.Seq](https://www.nuget.org/packages/Serilog.Sinks.Seq)
