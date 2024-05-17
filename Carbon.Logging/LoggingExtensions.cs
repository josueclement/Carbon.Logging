using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Carbon.Logging;

/// <summary>
/// Logging extensions for NLog
/// </summary>
public static class LoggingExtensions
{
    /// <summary>
    /// Adds NLog to the service collection
    /// </summary>
    /// <param name="services"></param>
    public static void AddNLog(this IServiceCollection services)
    {
        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.SetMinimumLevel(LogLevel.Trace);
            builder.AddNLog();
        });
    }
}