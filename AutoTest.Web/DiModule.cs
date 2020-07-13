using AutoTest.Web.Authorization;
using Microsoft.Extensions.Configuration;

namespace AutoTest.Web
{
    using AutoTest.Persistence;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class DiModule
    {
        public static void AddWeb(this IServiceCollection collection, IConfiguration configuration)
        {
            var config = configuration.GetSection("Cosmos");
            var endpoint = config.GetValue<string>("Endpoint");
            var key = config.GetValue<string>("Key");
            System.Diagnostics.Trace.TraceInformation($"Cosmos config, endpoint: {endpoint} key: {key}");
            collection.AddScoped<IAuthorizationHandler, MarshalRequirementHandler>();
            collection.AddScoped<IAuthorizationHandler, ClubAdminRequirementHandler>();
            collection.AddDbContext<AutoTestContext>(o => o.UseCosmos(
                endpoint,
                key,
                databaseName: "AutoTestDB"));
        }
    }
}
