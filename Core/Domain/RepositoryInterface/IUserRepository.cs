using Core.Domain.IdentityEntities;
using Core.Models.User;
using Microsoft.AspNetCore.Identity;

namespace Core.Domain.RepositoryInterface
{
   public interface IUserRepository
   {
      Task<ApplicationUser> GetUserAsync();
      Task<IdentityResult> ChangePasswordAsync(ChangePasswordRequest request, ApplicationUser user, CancellationToken cancellationToken);
      Task<IdentityResult> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken);
      Task<string> GetResetPasswordTokenAsync(ApplicationUser user);
      Task<IdentityResult> UpdateUserAsync(UpdateUserRequest request, CancellationToken cancellationToken);
   }
}