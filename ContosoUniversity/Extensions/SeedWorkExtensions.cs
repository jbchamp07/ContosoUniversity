using ContosoUniversity.Data;
using ContosoUniversity.Seedwork;
using Microsoft.Extensions.DependencyInjection;

namespace ContosoUniversity.Extensions
{
    public static class SeedWorkExtensions
    {

        public static IServiceCollection AddSeedwork(this IServiceCollection services)
        {
            services.AddScoped(typeof(IStudentsRepository), typeof(StudentsRepository));
            return services;
        }

    }
}
