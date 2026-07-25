using AutoTest.Persistence;
using AutoTest.Service.Interfaces;
using AutoTest.Service.ResultCalculation;
using AutoTest.Web.Authorization.Handlers;
using AutoTest.Web.Hubs;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AutoTest.Web;

public static class DiModule
{
    public static void AddWeb(this IServiceCollection collection, IConfiguration configuration)
    {
        var config = configuration.GetSection("Cosmos");
        var endpoint = config.GetValue<string>("Endpoint") ?? "";
        var key = config.GetValue<string>("Key") ?? "";
        var storageConfig = configuration.GetSection("Storage");
        var connectionString = storageConfig.GetValue<string>("ConnectionString") ?? "";
        collection.AddScoped<IAuthorizationHandler, MarshalRequirementHandler>();
        collection.AddScoped<IAuthorizationHandler, ClubAdminRequirementHandler>();
        collection.AddScoped<IAuthorizationHandler, SelfRequirementHandler>();
        collection.AddScoped<IAuthorizationHandler, ClubAdminOrSelfRequirementSelfHandler>();
        collection.AddScoped<IAuthorizationHandler, ClubAdminOrSelfRequirementClubAdminHandler>();
        collection.AddScoped<IEventNotifier, EventNotifier>();
        collection.AddScoped<IAuthorisationNotifier, AuthorisationNotifier>();
        collection.AddScoped<ITotalTimeCalculator, AutoTestTotalTimeCalculator>();
        collection.AddSingleton(new BlobContainerClient(connectionString, "eventfiles"));
        collection.AddDbContext<AutoTestContext>(o => o.UseCosmos(
            endpoint,
            key,
            "AutoTestDB"));
    }
}
