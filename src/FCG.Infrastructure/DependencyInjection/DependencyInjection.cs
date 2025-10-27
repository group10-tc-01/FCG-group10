using FCG.Domain.Repositories;
using FCG.Domain.Repositories.GamesRepository;
using FCG.Domain.Repositories.LibraryRepository;
using FCG.Domain.Repositories.RefreshTokenRepository;
using FCG.Domain.Repositories.UserRepository;
using FCG.Domain.Repositories.WalletRepository;
using FCG.Domain.Services;
using FCG.Infrastructure.Persistance;
using FCG.Infrastructure.Persistance.Repositories;
using FCG.Infrastructure.Services.Authentication;
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
            services.AddServices();

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

            services.AddScoped<IReadOnlyUserRepository, UserRepository>();
            services.AddScoped<IWriteOnlyUserRepository, UserRepository>();

            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            services.AddScoped<IWriteOnlyGameRepository, GameRepository>();
            services.AddScoped<IReadOnlyGameRepository, GameRepository>();

            services.AddScoped<IWriteOnlyLibraryRepository, LibraryRepository>();
            services.AddScoped<IWriteOnlyWalletRepository, WalletRepository>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPasswordEncrypter, PasswordEncrypterService>();
        }
    }
}
