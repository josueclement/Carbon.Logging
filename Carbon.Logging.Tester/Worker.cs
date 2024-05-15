using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Targets;

namespace Carbon.Logging.Tester;

public class Worker
{
    private readonly ILogger<Worker> _mainLogger;
    private readonly ILogger _logger;
    private readonly ILogger _logger2;
    // private readonly ILogger _logger3;

    public Worker(IServiceProvider provider, NLogLoggerProviderBuilder builder, ILogger<Worker> mainLogger)
    {
        _mainLogger = mainLogger;
        builder.AddColoredConsoleTarget("console", config =>
        {
            config.Layout = builder.DefaultLayout;
            var rule = new ConsoleRowHighlightingRule
            {
                Condition = "level == LogLevel.Debug",
                ForegroundColor = ConsoleOutputColor.DarkYellow
            };
            config.RowHighlightingRules.Add(rule);

        });
        builder.AddFileTarget("file", config =>
        {
            config.Layout = builder.DefaultLayout;
            config.FileName = "${basedir}/Log/${Logger}.log";
        });
        _logger = builder.BuildLogger("mylogger");

        _logger2 = provider.GetRequiredService<NLogLoggerProviderBuilder>().AddFileTarget("file", config =>
        {
            config.Layout = builder.DefaultLayout;
            config.FileName = "${basedir}/Log/${Logger}.log";
        }).BuildLogger("logger2");

        // _logger3 = provider.GetRequiredService<NLogLoggerProviderBuilder>().BuildLoggerFromConfigFile(
        //     "test",
        //     @"C:\Temp\myConfig.config");
    }

    public void DoSomething()
    {
        _mainLogger.Log(LogLevel.Trace, "Test from main logger :)");
        _logger.Log(LogLevel.Trace, "this is a test trace message with value {value}", 12.2);
        _logger.Log(LogLevel.Debug, "this is a test debug message");
        _logger.Log(LogLevel.Information, "this is a test info message");
        _logger.Log(LogLevel.Warning, "this is a test warning message");
        _logger2.Log(LogLevel.Warning, "test warning");
        // _logger3.Log(LogLevel.Information, "Hello from logger 3 hehe");
        try
        {
            BadMethod();
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, ex, "this is a test error message"); 
            _logger.Log(LogLevel.Critical, ex, "this is a test critical message"); 
        }
    }

    private void BadMethod()
    {
        throw new Exception("Bad method called");
    }
}