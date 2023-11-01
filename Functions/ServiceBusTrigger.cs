using Azure.Messaging.ServiceBus;
using Contracts;
using Core.Domain.RepositoryInterface;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Functions
{
    public class ServiceBusTrigger
    {
        private readonly ILogger<ServiceBusTrigger> _logger;
        private readonly IDataSeederRepository _dataSeederRepository;

        public ServiceBusTrigger(ILogger<ServiceBusTrigger> logger, IDataSeederRepository dataSeederRepository)
        {
            _logger = logger;
            _dataSeederRepository = dataSeederRepository;
        }

        [Function(nameof(ServiceBusTrigger))]
        public async Task Run([ServiceBusTrigger("location-seeder-queue", Connection = "ServiceBusQueueConnection")] ServiceBusReceivedMessage message, ServiceBusMessageActions messageActions, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Message ID: {id}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);
            string test = message.Body.ToString();
            LocationMessageModel location = JsonConvert.DeserializeObject<LocationMessageModel>(test);

            await _dataSeederRepository.SeedingLocationRepositoryAsync(location, cancellationToken);
            Console.WriteLine(message.Body.ToString());
        }
    }
}
