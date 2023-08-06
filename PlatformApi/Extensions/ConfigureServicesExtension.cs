using Azure.Storage.Blobs;
using Core.Domain.IdentityEntities;
using Core.Domain.RepositoryInterface;
using Core.Services;
using Infrastructure.DbContext;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PlatformApi.Filters;
using PlatformApi.Middlewares;
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
            options.Filters.Add(typeof(AuthorizationFilter));
         }).AddNewtonsoftJson();
         services.AddHttpContextAccessor();

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

         // Cors
         services.AddCors(options =>
         {
            options.AddPolicy("AllowSpecificOrigin",
               builder => builder
               .WithOrigins("http://localhost:3000", "http://localhost:3001", "http://127.0.0.1:3001")
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

         // Add Middleware
         services.AddSingleton<IAuthorizationMiddlewareResultHandler, UnauthorizedHandler>();
         services.AddTransient<IAuthorizationMiddlewareResultHandler, CustomAuthorizationMiddlewareResultHandler>();

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

         services.Configure<IdentityOptions>(options =>
            {
               options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2); // Set lockout duration to 30 minutes
               options.Lockout.MaxFailedAccessAttempts = 5; // Set maximum number of failed login attempts before lockout
            });

         return services;
      }
   }
}