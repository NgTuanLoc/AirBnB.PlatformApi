using AutoMapper;
using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using Core.Utils;
using Microsoft.AspNetCore.Authorization;
using PlatformApi.Filters;
using PlatformApi.Middlewares;

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

            string domainOriginString = configuration["AllowSpecificOrigin"] ?? "*";
            // Cors
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                builder => builder
                .WithOrigins(domainOriginString.Split(","))
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            });

            // BlobStorage
            services.AddScoped(_ =>
            {
                var connection = configuration.GetConnectionString("BlobStorageConnection");
                return new BlobServiceClient(connection);
            });

            // AutoMapper
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(new MappingProfiles());
            });
            var mapperObject = mapperConfig.CreateMapper();
            services.AddSingleton(mapperObject);

            // Service Bus
            services.AddScoped(x =>
            {
                var connection = configuration.GetConnectionString("ServiceBusConnection");
                return new ServiceBusClient(connection);
            });

            // Add Middleware
            services.AddTransient<IAuthorizationMiddlewareResultHandler, CustomAuthorizationMiddlewareResultHandler>();

            return services;
        }
    }
}