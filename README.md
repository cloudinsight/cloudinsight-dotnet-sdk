Cloud Insight SDK for C#
================

[![Build status](https://ci.appveyor.com/api/projects/status/aiwcurj3qfn5n46s?svg=true)](https://ci.appveyor.com/project/startover/cloudinsight-dotnet-sdk)
[![NuGet Version](http://img.shields.io/nuget/v/cloudinsight-dotnet-sdk.svg?style=flat)](https://www.nuget.org/packages/cloudinsight-dotnet-sdk/)

cloudinsight-dotnet-sdk is a fork of Goncalo Pereira's [CSharp Statsd client](https://github.com/Pereingo/statsd-csharp-client).

Installation
------------

Grab the [package from NuGet](https://nuget.org/packages/cloudinsight-dotnet-sdk/), or get the source from here and build it yourself.

Quick Start Guide
-----------------

At start of your app, configure the `CloudInsightStatsd` class like this:

``` C#
// The code is located under the StatsdClient namespace
using StatsdClient;

// ...

var statsdConfig = new StatsdConfig
{
    StatsdServerName = "127.0.0.1", // The hostname or address of the StatsD server
    StatsdPort = 8251, // Optional; default is 8251
    Prefix = "myApp" // Optional; the string that is prepended to all metrics, by default no prefix will be prepended
};

StatsdClient.CloudInsightStatsd.Configure(statsdConfig);
```

Then start instrumenting your code:

``` C#
// Increment a counter by 1
CloudInsightStatsd.Increment("eventname");

// Decrement a counter by 1
CloudInsightStatsd.Decrement("eventname");

// Increment a counter by a specific value
CloudInsightStatsd.Counter("page.views", page.views);

// Record a gauge
CloudInsightStatsd.Gauge("gas_tank.level", 0.75);

// Sample a histogram
CloudInsightStatsd.Histogram("file.size", file.size);

// Add elements to a set
CloudInsightStatsd.Set("users.unique", user.id);
CloudInsightStatsd.Set("users.unique", "email@string.com");

// Time a block of code
using (CloudInsightStatsd.StartTimer("stat-name"))
{
    // Some code
}

// Time an action
CloudInsightStatsd.Time(() => DoMagic(), "stat-name");

// Timing an action preserves its return value
var result = CloudInsightStatsd.Time(() => GetResult(), "stat-name");

// Every metric type supports tags and sample rates
CloudInsightStatsd.Set("users.unique", user.id, tags: new[] {"country:canada"});
CloudInsightStatsd.Gauge("gas_tank.level", 0.75, sampleRate: 0.5, tags: new[] {"hybrid", "trial_1"});
using (CloudInsightStatsd.StartTimer("stat-name", sampleRate: 0.1))
{
    // Lots of code here
}
```

