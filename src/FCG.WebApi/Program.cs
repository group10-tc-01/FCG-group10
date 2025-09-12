using FCG.Application.DependencyInjection;
using FCG.Infrastructure.DependencyInjection;
using FCG.WebApi.DependencyInjection;
using FCG.WebApi.Middlewares;
using Microsoft.OpenApi.Models;
using System.Diagnostics.CodeAnalysis;

namespace FCG.WebApi
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        protected Program() { }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FCG - V1", Version = "v1.0" });
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "FCG - V2", Version = "v2.0" });
            });

            builder.Services.AddWebApi();
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure();

            var app = builder.Build();

            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.MapHealthChecks("/health");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();

        }
    }
}