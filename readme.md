# ASP.NET Core 6 (and 8!) Performance

> **NOTE:** This repo has an updated branch for .NET 8 -- it's the `net8-updates` branch.

This is the repository with the code associated with a Pluralsight course called "ASP.NET Core Performance".

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

## JMeter

An example of load testing with [JMeter](https://jmeter.apache.org/) was shown and the artifacts are in the `jmeter` directory.

To install JMeter:

1. You need **Java** installed.  Get it from here: [https://www.java.com/en/download/](https://www.java.com/en/download/)
1. Get the latest zipped version of JMeter from here (*if you're on Windows make sure to get the `zip` version!*): [https://jmeter.apache.org/download_jmeter.cgi](https://jmeter.apache.org/download_jmeter.cgi)
1. Once you've extracted the zip file contents, run `jmeter.bat` from the `bin` directory

**Pro Tip:** Add the `bin` directory mentioned above to your `PATH` environment variable. Feel free to put directory
anywhere you see fit.
