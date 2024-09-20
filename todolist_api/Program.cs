
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));

var app = builder.Build();

var rnd = new Random();

app.Use(async (context, next) => 
{
    foreach(var (key, value) in context.Request.Cookies )
    {
        context.Response.Cookies.Delete(key);
    }

    await next.Invoke(context);
});

app.MapGet("/", async (context) => 
{
    var sb = new StringBuilder();

    foreach(var (key, value) in context.Request.Cookies )
    {
        sb.AppendLine($"{key}:\t\t{value}");
    }

    await context.Response.WriteAsync(sb.ToString());
});

app.Run();

class FileLogger(string path) : ILogger, IDisposable
{
    string path = path;
    static object _lock = new();

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return this;
    }

    public void Dispose() {}

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        lock(_lock)
        {
            File.AppendAllText(path, formatter(state, exception) + Environment.NewLine);
        }
    }
}

class FileLoggerProvider(string path) : ILoggerProvider
{
    string path = path;
    public ILogger CreateLogger(string categoryName)
    {
        return new FileLogger(path);
    }

    public void Dispose() {}
}

static class FileLoggerExtensions
{
    public static ILoggingBuilder AddFile(this ILoggingBuilder builder, string path)
    {
        builder.AddProvider(new FileLoggerProvider(path));

        return builder;
    }
}