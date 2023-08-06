using Core.Domain.IdentityEntities;
using Infrastructure.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace PlatformApi.Extensions
{
    public static class ConfigureIdentityExtension
    {
        public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders()
                    .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
                    .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2); // Set lockout duration to 30 minutes
                options.Lockout.MaxFailedAccessAttempts = 5; // Set maximum number of failed login attempts before lockout
            });
            return services;
        }
    }
}