using System.Security.Claims;
using Core.Domain.IdentityEntities;
using Core.Domain.RepositoryInterface;
using Core.Exceptions;
using Core.Models.User;
using Microsoft.AspNetCore.Identity;

namespace Core.Services
{
   public interface IUserService
   {
      Task<ApplicationUser> GetUserService();
      Task<IdentityResult> ChangePasswordService(ChangePasswordRequest request, CancellationToken cancellationToken);
      Task<IdentityResult> ResetPasswordService(ResetPasswordRequest request, CancellationToken cancellationToken);
      Task<string> GetResetPasswordTokenService();
      Task<IdentityResult> UpdateUserService(UpdateUserRequest request, CancellationToken cancellationToken);
   }

   public class UserService : IUserService
   {
      private readonly UserManager<ApplicationUser> _userManager;
      private readonly IUserRepository _userRepository;
      public UserService(UserManager<ApplicationUser> userManager, IUserRepository userRepository)
      {
         _userManager = userManager;
         _userRepository = userRepository;
      }

      public async Task<ApplicationUser> GetUserService()
      {
         var user = await _userRepository.GetUserAsync();
         return user;
      }

      public async Task<IdentityResult> ChangePasswordService(ChangePasswordRequest request, CancellationToken cancellationToken)
      {
         var user = await _userRepository.GetUserAsync();
         var result = await _userRepository.ChangePasswordAsync(request, user, cancellationToken);
         return result;
      }

      public async Task<string> GetResetPasswordTokenService()
      {
         var user = await _userRepository.GetUserAsync();
         var token = await _userRepository.GetResetPasswordTokenAsync(user);
         return token;
      }

      public async Task<IdentityResult> ResetPasswordService(ResetPasswordRequest request, CancellationToken cancellationToken)
      {
         IdentityResult result = await _userRepository.ResetPasswordAsync(request, cancellationToken);
         return result;
      }

      public async Task<IdentityResult> UpdateUserService(UpdateUserRequest request, CancellationToken cancellationToken)
      {
         var result = await _userRepository.UpdateUserAsync(request, cancellationToken);
         return result;
      }
   }
}