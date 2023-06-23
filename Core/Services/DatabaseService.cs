using Core.Domain.RepositoryInterface;

namespace Core.Services
{
   public interface IDatabaseService
   {
      Task<string> RestoreService(CancellationToken cancellationToken);
      Task <string> SeedingService(CancellationToken cancellationToken);
   }
   public class DatabaseService : IDatabaseService
   {
      private readonly IDatabaseRepository _databaseRepository;
      public DatabaseService(IDatabaseRepository databaseRepository)
      {
         _databaseRepository = databaseRepository;
      }
      public async Task<string> RestoreService(CancellationToken cancellationToken)
      {
         var result = await _databaseRepository.RestoreAsync(cancellationToken);

         return result;
      }

        public async Task<string> SeedingService(CancellationToken cancellationToken)
        {
         var result = await _databaseRepository.SeedingAsync(cancellationToken);
         
         return result;
        }
    }
}