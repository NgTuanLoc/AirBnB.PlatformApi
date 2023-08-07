using Core.Domain.RepositoryInterface;
using Core.Services;
using Infrastructure.DbContext;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace PlatformApi.Extensions
{
   public static class ConfigureServicesExtension
   {
      public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
      {
         // Add DbContext Services
         services.AddDbContext<ApplicationDbContext>(
            options =>
            {
               var connection = configuration.GetConnectionString("DefaultConnection");

               // if (env.IsDevelopment())
               // {
               //    connection = configuration.GetConnectionString("LocalDatabaseConnectionString");
               // }
               if (connection != null)
               {
                  options.UseSqlServer(connection);
               }
            }
         );

         // Repositories Layer
         services.AddScoped<IDatabaseRepository, DatabaseRepository>();
         services.AddScoped<IImageRepository, ImageRepository>();
         services.AddScoped<IUserRepository, UserRepository>();
         services.AddScoped<ILocationRepository, LocationRepository>();
         services.AddScoped<IRoomRepository, RoomRepository>();
         services.AddScoped<IReservationRepository, ReservationRepository>();
         services.AddScoped<IReviewRepository, ReviewRepository>();

         // Services Layer
         services.AddScoped<IDatabaseService, DatabaseService>();
         services.AddScoped<IAuthService, AuthService>();
         services.AddScoped<IImageService, ImageService>();
         services.AddScoped<ILocationService, LocationService>();
         services.AddScoped<IUserService, UserService>();
         services.AddScoped<IRoomService, RoomService>();
         services.AddScoped<IReservationService, ReservationService>();
         services.AddScoped<IReviewService, ReviewService>();
         services.AddScoped<IRoleService, RoleService>();

         return services;
      }
   }
}