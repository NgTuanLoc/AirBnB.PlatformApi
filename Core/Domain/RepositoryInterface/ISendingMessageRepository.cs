namespace Core.Domain.RepositoryInterface
{
    public interface ISendingMessageRepository
    {
        Task<string> ReadJsonFileFromBlobStorageAsync(string blobName, string blobStorageContainer, CancellationToken cancellationToken);
        Task SendMessageInBatchAsync(List<string> messageList, int amountOfMessagesPerBatch, CancellationToken cancellationToken);

    }
}