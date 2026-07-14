using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using openbanking_mobile_bff.Common.Middleware;

namespace openbanking_mobile_bff.Tests.Common.Middleware;

public sealed class RequestLoggingMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_LogsRequestAndResponseDetails_AndCallsNext()
    {
        var logger = new TestLogger<RequestLoggingMiddleware>();
        var middleware = new RequestLoggingMiddleware(logger);
        var context = new DefaultHttpContext();
        context.Request.Method = "POST";
        context.Request.Path = "/api/payments";
        var nextCalled = false;

        await middleware.InvokeAsync(context, ctx =>
        {
            nextCalled = true;
            ctx.Response.StatusCode = StatusCodes.Status201Created;
            return Task.CompletedTask;
        });

        Assert.True(nextCalled);

        var entry = Assert.Single(logger.Entries);
        Assert.Equal(LogLevel.Information, entry.LogLevel);
        Assert.Equal("HTTP {Method} {Path} responded {StatusCode} in {ElapsedMs}ms", entry.State["{OriginalFormat}"]);
        Assert.Equal("POST", entry.State["Method"]);
        Assert.Equal("/api/payments", entry.State["Path"]?.ToString());
        Assert.Equal(StatusCodes.Status201Created, entry.State["StatusCode"]);
        Assert.True(Convert.ToInt64(entry.State["ElapsedMs"]) >= 0);
    }

    private sealed class TestLogger<TCategoryName> : ILogger<TCategoryName>
    {
        public List<LogEntry> Entries { get; } = [];

        public IDisposable BeginScope<TState>(TState state) where TState : notnull => NullScope.Instance;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            var stateValues = state as IEnumerable<KeyValuePair<string, object?>> ?? [];
            Entries.Add(new LogEntry(logLevel, stateValues.ToDictionary(kvp => kvp.Key, kvp => kvp.Value)));
        }
    }

    private sealed record LogEntry(LogLevel LogLevel, Dictionary<string, object?> State);

    private sealed class NullScope : IDisposable
    {
        public static readonly NullScope Instance = new();

        public void Dispose()
        {
        }
    }
}
