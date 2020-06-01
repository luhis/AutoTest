namespace AutoTest.Persistence
{
    using Microsoft.Extensions.DependencyInjection;

    public static class DiModule
    {
        public static void AddPersistence(this IServiceCollection services)
        {
            //services.AddScoped<IApartmentRepository, ApartmentRepository>();
            //services.AddScoped<IClientRepository, ClientRepository>();
            //services.AddScoped<IRealtorRepository, RealtorRepository>();

            services.AddDbContext<AutoTestContext>();
        }
    }
}