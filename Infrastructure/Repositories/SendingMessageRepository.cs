using System.Text;
using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using Core.Constants;
using Core.Domain.RepositoryInterface;
using Core.Exceptions;

namespace Infrastructure.Repositories
{
    public class SendingMessageRepository : ISendingMessageRepository
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ServiceBusClient _serviceBusClient;

        public SendingMessageRepository(BlobServiceClient blobServiceClient, ServiceBusClient serviceBusClient)
        {
            _blobServiceClient = blobServiceClient;
            _serviceBusClient = serviceBusClient;
        }
        public async Task<string> ReadJsonFileFromBlobStorageAsync(string blobName, string blobStorageContainer, CancellationToken cancellationToken)
        {
            var blobStorageContainerClient = _blobServiceClient.GetBlobContainerClient(blobStorageContainer);
            var blobClient = blobStorageContainerClient.GetBlobClient(blobName);

            if (!await blobClient.ExistsAsync(cancellationToken)) throw new NotFoundException($"Blob with the name {blobName} is not found!");

            using var response = await blobClient.OpenReadAsync(cancellationToken: cancellationToken);
            using var streamReader = new StreamReader(response);
            var jsonString = streamReader.ReadToEnd();
            return jsonString;
        }

        public async Task SendMessageInBatchAsync(List<string> messageList, int amountOfMessagesPerBatch, CancellationToken cancellationToken)
        {
            int amountOfBatch = (int)Math.Ceiling((double)messageList.Count / amountOfMessagesPerBatch);
            // ToDo: Create a sender for the desired queue
            ServiceBusSender sender = _serviceBusClient.CreateSender(ConfigConstants.LOCATION_SEEDER_QUEUE);

            // ToDo: Create a list to hold the messages
            List<ServiceBusMessage> asbMessageList = new();

            for (int i = 0; i < amountOfBatch; i++)
            {
                // ToDo: Add messages to the list
                ServiceBusMessageBatch batch = await sender.CreateMessageBatchAsync(cancellationToken);
                List<ServiceBusMessage> messageBatch = messageList
                                     .Skip(i * amountOfMessagesPerBatch)
                                     .Take(amountOfMessagesPerBatch)
                                     .Select(item =>
                                     {
                                         byte[] messageBytes = Encoding.UTF8.GetBytes(item);
                                         return new ServiceBusMessage(messageBytes);
                                     })
                                     .ToList();

                // ToDo: Add message to message batch
                foreach (ServiceBusMessage item in messageBatch)
                {
                    // ToDo: Try adding the message to the batch
                    if (!batch.TryAddMessage(item))
                    {
                        throw new ValidationException($"Failed when sending message in batch {i}");
                    }
                }
                await sender.SendMessagesAsync(batch, cancellationToken);
            }
            // ! Close the service bus client
            await _serviceBusClient.DisposeAsync();
        }
    }
}