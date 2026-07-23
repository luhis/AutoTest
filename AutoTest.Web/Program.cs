using System;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AutoTest.Web;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        var logger = host.Services.GetRequiredService<ILoggerFactory>().CreateLogger("AutoTest.Startup");
        try
        {
            logger.LogInformation("Starting AutoTest application...");
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Environment: {Environment}", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
            }

            using var scope = host.Services.CreateScope();
            var autoTestContext = scope.ServiceProvider.GetRequiredService<AutoTestContext>();
            logger.LogInformation("Seeding database...");
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
            await autoTestContext.SeedDatabaseAsync(cts.Token);
            logger.LogInformation("Database seeded successfully.");

            logger.LogInformation("Starting web host...");
            await host.RunAsync();
        }
        catch (OperationCanceledException)
        {
            logger.LogCritical("Cosmos DB was not ready within the timeout. Exiting.");
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Application terminated unexpectedly");
            throw;
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                webBuilder.ConfigureKestrel(serverOptions => serverOptions.AddServerHeader = false);
            });
}
