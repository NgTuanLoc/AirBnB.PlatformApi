namespace Core.Domain.RepositoryInterface
{
   public interface IDatabaseRepository
   {
      Task<string> RestoreAsync(CancellationToken cancellationToken);
      Task<string> SeedingAsync(CancellationToken cancellationToken);
   }
}