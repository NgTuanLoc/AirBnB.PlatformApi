using Azure.Messaging.ServiceBus;
using Contracts;
using Core.Domain.RepositoryInterface;
using Core.Exceptions;
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

        [Function("LocationSeederFunction")]
        public async Task LocationSeederFunction([ServiceBusTrigger("location-seeder-queue", Connection = "ServiceBusQueueConnection")] ServiceBusReceivedMessage message, ServiceBusMessageActions messageActions, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Message ID: {id}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);
            LocationMessageModel? location = JsonConvert.DeserializeObject<LocationMessageModel>(message.Body.ToString()) ?? throw new NotFoundException("Location is null");

            var newLocation = await _dataSeederRepository.SeedingLocationRepositoryAsync(location, cancellationToken);
            await _dataSeederRepository.SendMessageToCreateRoomAsync(newLocation, cancellationToken);
        }

        [Function("RoomSeederFunction")]
        public async Task RoomSeederFunction([ServiceBusTrigger("room-seeder-queue", Connection = "ServiceBusQueueConnection")] ServiceBusReceivedMessage message, ServiceBusMessageActions messageActions, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Message ID: {id}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);
            RoomMessageModel? room = JsonConvert.DeserializeObject<RoomMessageModel>(message.Body.ToString()) ?? throw new NotFoundException("Room is null");

            await _dataSeederRepository.SeedingRoomRepositoryAsync(room, cancellationToken);
        }
    }
}
