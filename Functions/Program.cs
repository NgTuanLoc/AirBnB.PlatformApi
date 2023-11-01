using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using Core.Constants;
using Core.Domain.IdentityEntities;
using Core.Domain.RepositoryInterface;
using Infrastructure.DbContext;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((hostContext, services) =>
    {
        services.AddDbContext<ApplicationDbContext>(
            options =>
            {
                var connection = hostContext.Configuration.GetConnectionString("DefaultConnection");

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

        services.AddScoped(_ =>
            {
                var connection = hostContext.Configuration.GetConnectionString("BlobStorageConnection");
                return new BlobServiceClient(connection);
            });

        // Service Bus
        services.AddScoped(x =>
        {
            var connection = hostContext.Configuration.GetConnectionString("ServiceBusConnection");
            return new ServiceBusClient(connection);
        });

        // Repositories Layer
        services.AddScoped<IDataSeederRepository, DataSeederRepository>();
        services.AddScoped<IImageRepository, ImageRepository>();
        services.AddScoped<ISendingMessageRepository, SendingMessageRepository>();
    })
    .Build();

host.Run();
