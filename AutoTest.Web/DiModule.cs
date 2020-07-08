﻿using AutoTest.Web.Authorization;

namespace AutoTest.Web
{
    using AutoTest.Persistence;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class DiModule
    {
        public static void AddWeb(this IServiceCollection collection)
        {
            collection.AddScoped<IAuthorizationHandler, MarshalRequirementHandler>();
            collection.AddScoped<IAuthorizationHandler, ClubAdminRequirementHandler>();
            collection.AddDbContext<AutoTestContext>(o => o.UseCosmos(
                "https://localhost:8081",
                "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                databaseName: "AutoTestDB"));
        }
    }
}
