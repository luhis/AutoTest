﻿namespace AutoTest.Persistence
{
    using Microsoft.Extensions.DependencyInjection;

    public static class DiModule
    {
        public static void AddPersistence(this IServiceCollection services)
        {
            services.AddDbContext<AutoTestContext>();
        }
    }
}