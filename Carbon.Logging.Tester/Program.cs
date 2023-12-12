using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Carbon.Logging.Tester;

abstract class Program
{
    static void Main()
    {
        var services = new ServiceCollection();

        services.AddTransient<NLogLoggerProviderBuilder>();
        services.AddTransient<Worker>();
        services.AddNLog();
        
        var provider = services.BuildServiceProvider();

        ILogger<Program> mainLogger = provider.GetRequiredService<ILogger<Program>>();
        mainLogger.Log(LogLevel.Information, "Starting app");
        
        var worker = provider.GetRequiredService<Worker>();
        worker.DoSomething();
    }
}