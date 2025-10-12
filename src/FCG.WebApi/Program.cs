using FCG.Application.DependencyInjection;
using FCG.Application.Services.Seeds;
using FCG.Infrastructure.DependencyInjection;
using FCG.Infrastructure.Logging;
using FCG.Infrastructure.Persistance;
using FCG.WebApi.DependencyInjection;
using FCG.WebApi.Middlewares;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics.CodeAnalysis;

namespace FCG.WebApi
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        protected Program() { }

        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddWebApi(builder.Configuration);
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddSerilogLogging(builder.Configuration);


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FCG API v1");
                    c.EnablePersistAuthorization();
                });
            }

            app.UseMiddleware<GlobalExceptionMiddleware>();

            app.MapHealthChecks("/health", new HealthCheckOptions
            {
                AllowCachingResponses = false,
                ResultStatusCodes =
                {
                    [HealthStatus.Healthy] = StatusCodes.Status200OK,
                    [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
                }

            });

            if (app.Environment.IsDevelopment())
            {
                await RunMigrationsAsync(app);
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static async Task RunMigrationsAsync(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<FcgDbContext>();
            var seeds = scope.ServiceProvider.GetServices<ISeed>();

            await dbContext.Database.MigrateAsync();

            foreach (var seed in seeds)
            {
                await seed.SeedAsync();
            }
        }
    }
}