using Core.Domain.IdentityEntities;
using Core.Exceptions;
using Core.Models.User;
using Microsoft.AspNetCore.Identity;

namespace Core.Services
{
   public interface IUserService
   {
      Task<IdentityResult> ChangePasswordService(ChangePasswordRequest request, ApplicationUser? user, CancellationToken cancellationToken);
      Task<IdentityResult> ResetPasswordService(ResetPasswordRequest request, CancellationToken cancellationToken);
      Task<string> GetResetPasswordTokenService(ApplicationUser? user);
      Task<IdentityResult> UpdateUserService(UpdateUserRequest request, CancellationToken cancellationToken);
   }

   public class UserService : IUserService
   {
      private readonly UserManager<ApplicationUser> _userManager;
      public UserService(UserManager<ApplicationUser> userManager)
      {
         _userManager = userManager;
      }

      public async Task<IdentityResult> ChangePasswordService(ChangePasswordRequest request, ApplicationUser? user, CancellationToken cancellationToken)
      {
         if (user == null)
         {
            throw new ValidationException("User not found !");
         }

         IdentityResult result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
         return result;
      }

      public async Task<string> GetResetPasswordTokenService(ApplicationUser? user)
      {
         if (user == null)
         {
            throw new ValidationException("User not found !");
         }
         var token = await _userManager.GeneratePasswordResetTokenAsync(user);
         return token;
      }

      public async Task<IdentityResult> ResetPasswordService(ResetPasswordRequest request, CancellationToken cancellationToken)
      {
         var user = await _userManager.FindByEmailAsync(request.Email);

         if (user == null)
         {
            throw new ValidationException("User not found !");
         }

         IdentityResult result = await _userManager.ResetPasswordAsync(user, request.ResetPasswordToken, request.NewPassword);
         return result;
      }

      public async Task<IdentityResult> UpdateUserService(UpdateUserRequest request, CancellationToken cancellationToken)
      {
         var user = await _userManager.FindByEmailAsync(request.Email);

         if (user == null)
         {
            throw new ValidationException("User not found !");
         }

         // Update User
         if (request.PersonName != null)
         {
            user.PersonName = request.PersonName;
         }
         if (request.Address != null)
         {
            user.Address = request.Address;
         }
         if (request.ProfileImage != null)
         {
            user.ProfileImage = request.ProfileImage;
         }
         if (request.Description != null)
         {
            user.Description = request.Description;
         }
         if (request.IsMarried != null)
         {
            user.IsMarried = request.IsMarried;
         }

         var result = await _userManager.UpdateAsync(user);

         return result;
      }
   }
}