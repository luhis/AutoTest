using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using AutoTest.Domain.Repositories;
using AutoTest.Integration.Test.Tooling;
using AutoTest.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace AutoTest.Integration.Test.Fixtures
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        public CustomWebApplicationFactory()
        {
            this.Mr = new MockRepository(MockBehavior.Strict);
            this.IFileRepository = Mr.Create<IFileRepository>();
        }
        private static readonly IReadOnlyList<Type> ToRemove = new[]
        {
            typeof(DbContextOptions<AutoTestContext>)
        };

        public HttpClient GetUnAuthorisedClient()
            => this.CreateClient(
                new WebApplicationFactoryClientOptions() { AllowAutoRedirect = false });
        public MockRepository Mr { get; }
        public Mock<IFileRepository> IFileRepository { get; }

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

                services.AddSingleton(IFileRepository.Object);
                // Add ApplicationDbContext using an in-memory database for testing.
                services.AddDbContext<AutoTestContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTestingNoAuth");
                });

                // Build the service provider.
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database
                // context (ApplicationDbContext).
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
