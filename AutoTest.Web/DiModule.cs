namespace AutoTest.Web
{
    using System;
    using AutoTest.Persistence;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class DiModule
    {
        public static void AddWeb(this IServiceCollection collection)
        {
            //collection.AddScoped<IAuthorizationHandler, RealtorOrAdminRequirementHandler>();
            //collection.AddScoped<ISimpleRequirements, SimpleRequirements>();
            collection.AddSingleton(GetDbOptions);
        }

        private static DbContextOptions<AutoTestContext> GetDbOptions(IServiceProvider a) => new DbContextOptionsBuilder<AutoTestContext>()
            .UseSqlite(a.GetService<IConfiguration>().GetSection("DbPath").Get<string>()).Options;
    }
}