using System.Collections.Generic;
using System;
using System.Linq;
using AutoTest.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace AutoTest.Integration.Test.Fixtures
{
    public class AuthdCustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        private static readonly IReadOnlyList<Type> ToRemove = new[]
        {
            typeof(DbContextOptions<AutoTestContext>)
        };

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var dbContextDescriptor = services.Single(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<AutoTestContext>));

                services.Remove(dbContextDescriptor);

                services.AddDbContext<AutoTestContext>((container, options) =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });
                services.AddMvc(options =>
                {
                    {
                        options.Filters.Clear();
                        options.Filters.Add(new AllowAnonymousFilter());
                        options.Filters.Add(new FakeUserFilter());
                    }
                })
                .AddApplicationPart(typeof(TStartup).Assembly);
            });

            builder.UseEnvironment("Development");
        }
    }
}
