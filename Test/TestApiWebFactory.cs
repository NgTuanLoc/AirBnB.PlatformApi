using Infrastructure.DbContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Azure.Storage.Blobs;
using Moq;

namespace Test
{
    public class TestApiWebFactory : WebApplicationFactory<Program>
    {
        private readonly BlobServiceClient _mockBlobServiceClient;

        public TestApiWebFactory()
        {
            _mockBlobServiceClient = new Mock<BlobServiceClient>().Object; // Create and configure your mock here
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.UseEnvironment("Test");

            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(temp => temp.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("DatbaseForTesting");
                });
                services.AddSingleton(_mockBlobServiceClient);
            });
        }
        protected override void ConfigureClient(HttpClient client)
        {
            client.DefaultRequestHeaders.Add(HttpRequestHeader.Authorization.ToString(), "Bearer #{ApiKey}#");
        }
        public new HttpClient CreateClient(WebApplicationFactoryClientOptions options)
        {
            // var httpClient = new HttpClient
            // {
            //     BaseAddress = baseUrl
            // };
            var httpClient = new HttpClient();
            ConfigureClient(httpClient);
            return httpClient;
        }
    }
}