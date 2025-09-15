using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.Domain.Entities;
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

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test").ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<FcgDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

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

        private static void StartDatabase(FcgDbContext context)
        {
            var itemsQuantity = 2;

            Log.Information($"Creating {itemsQuantity} items for integrated test");

            try
            {
                var example = CreateExample(context, itemsQuantity);

            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while seeding the database with test data. Error: {Message}", ex.Message);
                throw;
            }
        }

        private static List<Example> CreateExample(FcgDbContext context, int itemsQuantity)
        {
            var examples = new List<Example>();

            for (int i = 1; i <= itemsQuantity; i++)
            {
                var example = ExampleBuilder.Build();
                examples.Add(example);
            }
            context.Examples.AddRange(examples);
            context.SaveChanges();

            Log.Information("Created {Count} examples", examples.Count);

            return examples;
        }
    }
}
