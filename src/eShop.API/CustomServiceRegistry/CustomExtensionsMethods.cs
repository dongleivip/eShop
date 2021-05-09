using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;

namespace Microsoft.Extensions.DependencyInjection
{
    static class CustomExtensionsMethods
    {
        public static IServiceCollection AddCustomMvc(this IServiceCollection services)
        {
            // Add framework services.
            services.AddControllers()
                // Added for functional tests
                .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            return services;
        }

        public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            var hcBuilder = services.AddHealthChecks();

            hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());

            // Todo: health check for database

            return services;
        }

        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                // options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "eShop Learning",
                    Version = "v1",
                    Description = "The eShop API"
                });
                // Todo: Auth for Swagger
                //options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                //{
                //    Type = SecuritySchemeType.OAuth2,
                //    Flows = new OpenApiOAuthFlows()
                //    {
                //        Implicit = new OpenApiOAuthFlow()
                //        {
                //            AuthorizationUrl = new Uri($"{configuration.GetValue<string>("IdentityUrl")}/connect/authorize"),
                //            TokenUrl = new Uri($"{configuration.GetValue<string>("IdentityUrl")}/connect/token"),
                //            Scopes = new Dictionary<string, string>()
                //            {
                //                { "orders", "Ordering API" }
                //            }
                //        }
                //    }
                //});

                // Todo: options for AuthorizeCheckOperationFilter
            });

            return services;
        }
    }
}
