using Infrastructure.DbContext;
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

                Console.WriteLine(connection);

                if (connection != null)
                {
                    options.UseSqlServer(connection);
                }
            }
        );
    })
    .Build();

host.Run();
