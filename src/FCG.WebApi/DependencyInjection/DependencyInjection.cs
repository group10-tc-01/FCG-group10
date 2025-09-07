using Asp.Versioning;

namespace FCG.WebApi.DependencyInjection
{

    public static class DependencyInjection
    {
        public static IServiceCollection AddWebApi(this IServiceCollection services)
        {
            services.AddVersioning();
            services.AddHealthChecks();

            return services;
        }

        public static void AddVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("X-Api-Version")
                );
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
        }

    }
}
