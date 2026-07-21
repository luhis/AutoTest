using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace AutoTest.Integration.Test.Fixtures;

public class AuthenticatedWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup>
    where TStartup : class
{
    const string TestScheme = nameof(TestScheme);

    public HttpClient GetAuthorisedClient()
    {
        var c = CreateClient(
            new WebApplicationFactoryClientOptions() { AllowAutoRedirect = false });
        c.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(scheme: TestScheme);
        return c;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            TestDatabaseInitializer.ConfigureInMemoryDatabase(services, "InMemoryDbForTestingAuth");

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = TestScheme;
                o.DefaultChallengeScheme = TestScheme;
            }).AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
            TestScheme, options => { });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            TestDatabaseInitializer.SeedDatabase(scope.ServiceProvider, typeof(AuthenticatedWebApplicationFactory<TStartup>));
        });
    }
}
