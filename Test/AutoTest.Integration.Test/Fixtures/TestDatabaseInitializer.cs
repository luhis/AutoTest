using System;
using System.Linq;
using System.Threading;
using AutoTest.Domain.Repositories;
using AutoTest.Integration.Test.Tooling;
using AutoTest.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace AutoTest.Integration.Test.Fixtures;

public static class TestDatabaseInitializer
{
    public static void ConfigureInMemoryDatabase(IServiceCollection services, string dbName)
    {
        var descriptors = services.Where(d =>
            d.ServiceType == typeof(DbContextOptions<AutoTestContext>) ||
            d.ServiceType == typeof(AutoTestContext) ||
            (d.ServiceType.Namespace != null && d.ServiceType.Namespace.StartsWith("Microsoft.EntityFrameworkCore", StringComparison.Ordinal))).ToList();
        foreach (var d in descriptors)
        {
            services.Remove(d);
        }

        services.AddDbContext<AutoTestContext>(options =>
        {
            options.UseInMemoryDatabase(dbName);
        });
    }

    public static void ConfigureMockFileRepository(IServiceCollection services)
    {
        var fileRepo = new Mock<IFileRepository>(MockBehavior.Strict);
        fileRepo.Setup(a => a.GetMaps(It.IsAny<ulong>(), It.IsAny<CancellationToken>())).ReturnsAsync("");
        fileRepo.Setup(a => a.HasMaps(It.IsAny<ulong>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        fileRepo.Setup(a => a.GetRegs(It.IsAny<ulong>(), It.IsAny<CancellationToken>())).ReturnsAsync("");
        fileRepo.Setup(a => a.HasRegs(It.IsAny<ulong>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        fileRepo.Setup(a => a.SaveMaps(It.IsAny<ulong>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync("");
        fileRepo.Setup(a => a.SaveRegs(It.IsAny<ulong>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync("");
        services.AddSingleton(fileRepo.Object);
    }

    public static void SeedDatabase(IServiceProvider scopedServices, Type loggerType)
    {
        var db = scopedServices.GetRequiredService<AutoTestContext>();
        var logger = (ILogger)scopedServices
            .GetRequiredService(typeof(ILogger<>).MakeGenericType(loggerType));

        db.Database.EnsureCreated();

        try
        {
            DbInitialiser.InitializeDbForTests(db);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "An error occurred seeding the database with test messages. Error: {Message}",
                ex.Message);
        }
    }
}
