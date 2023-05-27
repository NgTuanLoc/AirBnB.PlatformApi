using Azure.Storage.Blobs;
using Core.Domain.IdentityEntities;
using Core.Services;
using Infrastructure.DbContext;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PlatformApi.Filters;
using PlatformApi.Policy;

namespace PlatformApi.Extensions
{
   public static class ConfigureServicesExtension
   {
      public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
      {
         // Add Base Services
         services.AddControllers(options =>
         {
            options.Filters.Add(typeof(ModelStateFilter));
            options.Filters.Add(typeof(CustomExceptionFilter));
         }).AddNewtonsoftJson();
         services.AddEndpointsApiExplorer();
         services.AddSwaggerGen();

         // BlobStorage
         services.AddScoped(_ =>
         {
            var connection = configuration.GetConnectionString("BlobStorageConnection");
            return new BlobServiceClient(connection);
         });

         // Add Middleware
         services.AddSingleton<IAuthorizationMiddlewareResultHandler, UnauthorizedHandler>();

         // Repositories Layer
         services.AddScoped<IImageRepository, ImageRepository>();

         // Services Layer
         services.AddScoped<IAuthService, AuthService>();
         services.AddScoped<IImageService, ImageService>();

         // Add DbContext Services
         services.AddDbContext<ApplicationDbContext>(
             options =>
             {
                var connection = configuration.GetConnectionString("DefaultConnection");
                if (connection != null)
                {
                   options.UseSqlServer(connection);
                }
             }
         );

         services.AddIdentity<ApplicationUser, ApplicationRole>()
           .AddEntityFrameworkStores<ApplicationDbContext>()
           .AddDefaultTokenProviders()
           .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
           .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();

         return services;
      }
   }
}