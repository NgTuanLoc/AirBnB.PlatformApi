using System.Net;
using Core.Domain.Entities;
using Infrastructure.DbContext;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Functions
{
    public class DataSeeder
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;

        public DataSeeder(ILoggerFactory loggerFactory, ApplicationDbContext context)
        {
            _logger = loggerFactory.CreateLogger<DataSeeder>();
            _context = context;
        }

        [Function("DataSeeder")]
        public async Task<List<string>> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var locationNameList = await _context.Location
                .Select(location => location.Name)
                .ToListAsync();

            return locationNameList;
        }
    }
}
