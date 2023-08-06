using AutoMapper;
using Azure.Storage.Blobs;
using Core.Utils.MapperProfiles;
using Microsoft.AspNetCore.Authorization;
using PlatformApi.Filters;
using PlatformApi.Middlewares;
using PlatformApi.Policy;

namespace PlatformApi.Extensions
{
    public static class ConfigureExternalServicesExtension
    {
        public static IServiceCollection ConfigureExternalServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            // Add Base Services
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ModelStateFilter));
                options.Filters.Add(typeof(CustomExceptionFilter));
                options.Filters.Add(typeof(AuthorizationFilter));
            }).AddNewtonsoftJson();
            services.AddHttpContextAccessor();

            // Cors
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                builder => builder
                .WithOrigins("http://localhost:3000", "http://localhost:3001")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            });

            // BlobStorage
            services.AddScoped(_ =>
            {
                var connection = configuration.GetConnectionString("BlobStorageConnection");
                if (env.IsDevelopment())
                {
                    connection = configuration.GetConnectionString("LocalBlobStorageConnection");
                }
                return new BlobServiceClient(connection);
            });

            // AutoMapper
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(new AuthMappingProfile());
            });
            var mapperObject = mapperConfig.CreateMapper();
            services.AddSingleton(mapperConfig);

            // Add Middleware
            services.AddSingleton<IAuthorizationMiddlewareResultHandler, UnauthorizedHandler>();
            services.AddTransient<IAuthorizationMiddlewareResultHandler, CustomAuthorizationMiddlewareResultHandler>();

            return services;
        }
    }
}