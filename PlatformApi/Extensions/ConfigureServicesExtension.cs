using Core.Domain.IdentityEntities;
using Core.Services;
using Infrastructure.DbContext;
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

         services.AddSingleton<IAuthorizationMiddlewareResultHandler, UnauthorizedHandler>();

         // Repositories Layer

         // Services Layer
         services.AddScoped<IAuthService, AuthService>();

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