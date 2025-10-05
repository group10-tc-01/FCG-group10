using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.CommomTestsUtilities.Builders.Services;
using FCG.Domain.Entities;
using FCG.Domain.Services;
using FCG.Infrastructure.Persistance;
using FCG.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace FCG.IntegratedTests.Configurations
{
    [ExcludeFromCodeCoverage]
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private DbConnection? _connection;
        public List<User> CreatedUsers { get; private set; } = [];
        public List<RefreshToken> CreatedRefreshTokens { get; private set; } = [];

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test").ConfigureServices(services =>
            {
                RemoveEntityFrameworkServices(services);
                RemovePasswordEncrypterService(services);

                _connection?.Dispose();
                _connection = new SqliteConnection("Data Source=:memory:");
                _connection.Open();

                services.AddDbContext<FcgDbContext>(options =>
                {
                    options.UseSqlite(_connection)
                            .EnableSensitiveDataLogging()
                            .EnableDetailedErrors();
                });

                EnsureDatabaseSeeded(services);
            });
        }

        private static void RemoveEntityFrameworkServices(IServiceCollection services)
        {
            var descriptorsToRemove = services.Where(d =>
                d.ServiceType == typeof(DbContextOptions<FcgDbContext>) ||
                d.ServiceType == typeof(FcgDbContext) ||
                d.ServiceType.Namespace?.StartsWith("Microsoft.EntityFrameworkCore") == true)
                .ToList();

            foreach (var descriptor in descriptorsToRemove)
            {
                services.Remove(descriptor);
            }
        }

        private static void RemovePasswordEncrypterService(IServiceCollection services)
        {
            var passwordEncrypterService = services.Where(service => service.ServiceType == typeof(IPasswordEncrypter));

            if (passwordEncrypterService.Any())
            {
                services.Remove(passwordEncrypterService.First());
            }

            services.AddScoped<IPasswordEncrypter>(_ => PasswordEncrypterServiceBuilder.Build());
        }

        private void EnsureDatabaseSeeded(IServiceCollection services)
        {
            using var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<FcgDbContext>();

            Log.Information("Seeding database for integrated tests");

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            StartDatabase(dbContext);
        }

        private void StartDatabase(FcgDbContext context)
        {
            var itemsQuantity = 2;

            Log.Information($"Creating {itemsQuantity} items for integrated test");

            CreatedUsers = CreateUser(context, itemsQuantity);
            CreatedRefreshTokens = CreateRefreshTokens(context, CreatedUsers);
        }

        private List<User> CreateUser(FcgDbContext context, int itemsQuantity)
        {
            var users = new List<User>();

            for (int i = 1; i <= itemsQuantity; i++)
            {
                var user = UserBuilder.Build();
                users.Add(user);
            }

            context.Users.AddRange(users);
            context.SaveChanges();
            Log.Information("Created {Count} users", users.Count);

            return users;
        }

        public List<RefreshToken> CreateRefreshTokens(FcgDbContext context, List<User> users)
        {
            var refreshTokens = new List<RefreshToken>();

            foreach (var user in users)
            {
                var refreshToken = RefreshTokenBuilder.BuildWithUserId(user.Id);
                refreshTokens.Add(refreshToken);
            }

            context.RefreshTokens.AddRange(refreshTokens);
            context.SaveChanges();
            Log.Information("Created {Count} refresh tokens", refreshTokens.Count);
            CreatedRefreshTokens = refreshTokens;
            return refreshTokens;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _connection?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
