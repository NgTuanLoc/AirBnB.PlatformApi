using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;

namespace PlatformApi.Extensions
{
    public static class ConfigureSwaggerExtension
    {
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                config.ApiVersionReader = new UrlSegmentApiVersionReader(); //Reads version number from request url at "apiVersion" constraint

                //config.ApiVersionReader = new QueryStringApiVersionReader(); //Reads version number from request query string called "api-version". Eg: api-version=1.0

                //config.ApiVersionReader = new HeaderApiVersionReader("api-version"); //Reads version number from request header called "api-version". Eg: api-version: 1.0

                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
            });
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV"; //v1
                options.SubstituteApiVersionInUrl = true;
            });
            services.AddEndpointsApiExplorer(); //Generates description for all endpoints
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "AirBnB API", Version = "v1" });
                option.SwaggerDoc("v2", new OpenApiInfo { Title = "AirBnB API", Version = "v2" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
            return services;
        }
    }
}