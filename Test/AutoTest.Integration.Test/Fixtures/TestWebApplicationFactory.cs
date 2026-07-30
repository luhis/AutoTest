using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

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
            TestDatabaseInitializer.ConfigureMockFileRepository(services);

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            TestDatabaseInitializer.SeedDatabase(scope.ServiceProvider, typeof(TestWebApplicationFactory<TStartup>));
        });
    }
}
