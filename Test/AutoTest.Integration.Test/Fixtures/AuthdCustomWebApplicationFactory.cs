using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using AutoTest.Integration.Test.Tooling;
using AutoTest.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AutoTest.Integration.Test.Fixtures
{
    public class AuthdCustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        const string TestScheme = nameof(TestScheme);
        private static readonly IReadOnlyList<Type> ToRemove = new[]
        {
            typeof(DbContextOptions<AutoTestContext>)
        };

        public HttpClient GetAuthorisedClient()
        {
            var c = this.CreateClient(
                        new WebApplicationFactoryClientOptions() { AllowAutoRedirect = false });
            c.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(scheme: TestScheme);
            return c;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.Where(
                    d => ToRemove.Contains(d.ServiceType)).ToList();
                foreach (var d in descriptor)
                {
                    services.Remove(d);
                }

                services.AddDbContext<AutoTestContext>((container, options) =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTestingAuth");
                });
                services.AddAuthentication(o =>
                {
                    o.DefaultAuthenticateScheme = TestScheme;
                    o.DefaultChallengeScheme = TestScheme;
                }).AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                TestScheme, options => { });
                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<AutoTestContext>();
                var logger = scopedServices
                    .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                // Ensure the database is created.
                db.Database.EnsureCreated();

                try
                {
                    // Seed the database with test data.
                    DbInitialiser.InitializeDbForTests(db);
                }
                catch (Exception ex)
                {
                    logger.LogError(
                        ex,
                        "An error occurred seeding the database with test messages. Error: {Message}",
                        ex.Message);
                }
            });
        }
    }
}
