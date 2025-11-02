using FCG.Domain.Repositories;
using FCG.Domain.Repositories.GamesRepository;
using FCG.Domain.Repositories.LibraryRepository;
using FCG.Domain.Repositories.PromotionRepository;
using FCG.Domain.Repositories.RefreshTokenRepository;
using FCG.Domain.Repositories.UserRepository;
using FCG.Domain.Repositories.WalletRepository;
using FCG.Domain.Services;
using FCG.Infrastructure.Persistance;
using FCG.Infrastructure.Persistance.Repositories;
using FCG.Infrastructure.Services;
using FCG.Infrastructure.Services.Authentication;
using FCG.Infrastructure.Services.CorrelationId;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Diagnostics.CodeAnalysis;

namespace FCG.Infrastructure.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddSqlServer(configuration);
            services.AddRepositories();
            services.AddServices();
            services.AddSerilogLogging(configuration);

            return services;
        }

        private static void AddSqlServer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<FcgDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IReadOnlyUserRepository, UserRepository>();
            services.AddScoped<IWriteOnlyUserRepository, UserRepository>();

            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            services.AddScoped<IWriteOnlyGameRepository, GameRepository>();
            services.AddScoped<IReadOnlyGameRepository, GameRepository>();

            services.AddScoped<IReadOnlyPromotionRepository, PromotionRepository>();
            services.AddScoped<IWriteOnlyPromotionRepository, PromotionRepository>();

            services.AddScoped<IReadOnlyLibraryRepository, LibraryRepository>();
            services.AddScoped<IWriteOnlyLibraryRepository, LibraryRepository>();

            services.AddScoped<IReadOnlyWalletRepository, WalletRepository>();
            services.AddScoped<IWriteOnlyWalletRepository, WalletRepository>();
        }

        private static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<ILoggedUser, LoggedUser>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPasswordEncrypter, PasswordEncrypterService>();
            services.AddScoped<IAdminSeedService, AdminSeedService>();
            services.AddScoped<ICorrelationIdProvider, CorrelationIdProvider>();
        }

        private static void AddSerilogLogging(this IServiceCollection services, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{CorrelationId}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.Seq(configuration["Serilog:SeqUrl"] ?? "http://localhost:5341")
                .CreateLogger();

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog();
            });
        }
    }
}
