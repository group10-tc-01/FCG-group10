using Microsoft.OpenApi.Models;
using System.Diagnostics.CodeAnalysis;

namespace FCG.WebApi.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerWithSecurity(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Insira o token JWT no formato: Bearer {token}"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                options.SwaggerDoc("v1", new OpenApiInfo { Title = "FCG - V1", Version = "v1.0" });
                options.SwaggerDoc("v2", new OpenApiInfo { Title = "FCG - V2", Version = "v2.0" });
            });

            return services;
        }
    }
}
