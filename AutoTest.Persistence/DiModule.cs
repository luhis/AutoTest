using AutoTest.Domain.Repositories;
using AutoTest.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AutoTest.Persistence;


public static class DiModule
{
    public static void AddPersistence(this IServiceCollection services)
    {
        services.AddScoped<IEntrantsRepository, EntrantsRepository>();
        services.AddScoped<IEventsRepository, EventsRepository>();
        services.AddScoped<IClubsRepository, ClubRepository>();
        services.AddScoped<ITestRunsRepository, TestRunsRepository>();
        services.AddScoped<IMarshalsRepository, MarshalsRepository>();
        services.AddScoped<IProfileRepository, ProfileRepository>();
        services.AddScoped<INotificationsRepository, NotificationRepository>();
        services.AddScoped<IFileRepository, FileRepository>();
    }
}
