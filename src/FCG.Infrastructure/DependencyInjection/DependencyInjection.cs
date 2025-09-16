using FCG.Domain.Repositories;
using FCG.Domain.Repositories.ExampleRepository;
using FCG.Infrastructure.Persistance;
using FCG.Infrastructure.Persistance.Repositories;
using FCG.Infrastructure.Persistance.Repositories.ExampleRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace FCG.Infrastructure.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSqlServer(configuration);
            services.AddRepositories();

            return services;
        }

        public static void AddSqlServer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<FcgDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IWriteOnlyExampleRepository, ExampleRepository>();
        }

    }
}
