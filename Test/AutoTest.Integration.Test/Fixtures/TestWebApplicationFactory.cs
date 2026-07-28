using System;
using System.Net.Http;
using System.Threading;
using AutoTest.Domain.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace AutoTest.Integration.Test.Fixtures;

public class TestWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup>
    where TStartup : class
{
    public HttpClient GetUnAuthorisedClient()
        => CreateClient(
            new WebApplicationFactoryClientOptions() { AllowAutoRedirect = false });

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            TestDatabaseInitializer.ConfigureInMemoryDatabase(services, $"InMemoryDbForTestingNoAuth_{Guid.NewGuid()}");

            var fileRepo = new Mock<IFileRepository>();
            fileRepo.Setup(a => a.GetMaps(It.IsAny<ulong>(), It.IsAny<CancellationToken>())).ReturnsAsync("");
            fileRepo.Setup(a => a.GetRegs(It.IsAny<ulong>(), It.IsAny<CancellationToken>())).ReturnsAsync("");
            fileRepo.Setup(a => a.SaveMaps(It.IsAny<ulong>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync("");
            fileRepo.Setup(a => a.SaveRegs(It.IsAny<ulong>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync("");
            services.AddSingleton(fileRepo.Object);

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            TestDatabaseInitializer.SeedDatabase(scope.ServiceProvider, typeof(TestWebApplicationFactory<TStartup>));
        });
    }
}
