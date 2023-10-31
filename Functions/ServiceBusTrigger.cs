using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Functions
{
    public class ServiceBusTrigger
    {
        private readonly ILogger<ServiceBusTrigger> _logger;

        public ServiceBusTrigger(ILogger<ServiceBusTrigger> logger)
        {
            _logger = logger;
        }

        [Function(nameof(ServiceBusTrigger))]
        public void Run([ServiceBusTrigger("data-seeder-queue", Connection = "ServiceBusQueueConnection")] ServiceBusReceivedMessage message)
        {
            _logger.LogInformation("Message ID: {id}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);
            Console.WriteLine(message.Body.ToString());
            _logger.LogInformation("Hello");
        }
    }
}
