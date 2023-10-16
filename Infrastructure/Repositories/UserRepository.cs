using Core.Domain.IdentityEntities;
using Core.Domain.RepositoryInterface;
using Core.Exceptions;
using Core.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories
{
   public class UserRepository : IUserRepository
   {
      private readonly IHttpContextAccessor _httpContextAccessor;
      private readonly UserManager<ApplicationUser> _userManager;
      public UserRepository(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
      {
         _httpContextAccessor = httpContextAccessor;
         _userManager = userManager;
      }

      public async Task<ApplicationUser> GetUserAsync()
      {
         HttpContext context = _httpContextAccessor.HttpContext;
         var email = (context?.User?.Identity?.Name) ?? throw new ValidationException("User not found !");
         var user = await _userManager.FindByEmailAsync(email) ?? throw new ValidationException("User not found !");
         return user;
      }

      public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordRequest request, ApplicationUser user, CancellationToken cancellationToken)
      {
         IdentityResult result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
         return result;
      }

      public async Task<string> GetResetPasswordTokenAsync(ApplicationUser user)
      {
         var token = await _userManager.GeneratePasswordResetTokenAsync(user);
         return token;
      }

      public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken)
      {
         var user = await _userManager.FindByEmailAsync(request.Email) ?? throw new ValidationException("User not found !");
         IdentityResult result = await _userManager.ResetPasswordAsync(user, request.ResetPasswordToken, request.NewPassword);
         return result;
      }

      public async Task<IdentityResult> UpdateUserAsync(UpdateUserRequest request, CancellationToken cancellationToken)
      {
         var user = await _userManager.FindByEmailAsync(request.Email) ?? throw new ValidationException("User not found !");

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