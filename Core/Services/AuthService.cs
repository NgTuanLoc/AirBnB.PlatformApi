using Core.Exceptions;
using Core.Models.Auth;
using Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity;

namespace Core.Services
{
   public interface IAuthService
   {
      Task<string> RegisterService(RegisterRequest request, CancellationToken cancellationToken);
      Task<ApplicationUser> LoginService(LoginRequest request, CancellationToken cancellationToken);
      Task<string> LogoutService(CancellationToken cancellationToken);
   }
   public class AuthService : IAuthService
   {
      private readonly UserManager<ApplicationUser> _userManager;
      private readonly SignInManager<ApplicationUser> _signInManager;
      public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManger)
      {
         _userManager = userManager;
         _signInManager = signInManger;
      }
      public async Task<string> RegisterService(RegisterRequest request, CancellationToken cancellationToken)
      {
         ApplicationUser user = new ApplicationUser() { Email = request.Email, PhoneNumber = request.Phone, UserName = request.Email, PersonName = request.PersonName };

         IdentityResult result = await _userManager.CreateAsync(user, request.Password);

         if (!result.Succeeded)
         {
            IDictionary<string, string[]> failures = new Dictionary<string, string[]>();
            foreach (IdentityError error in result.Errors)
            {
               failures.Add(error.Code, new[] { error.Description });
            }
            throw new ValidationException(failures);
         }

         await _signInManager.SignInAsync(user, isPersistent: true);
         return "register succeed";
      }
      public async Task<ApplicationUser> LoginService(LoginRequest request, CancellationToken cancellationToken)
      {
         var user = await _userManager.FindByNameAsync(request.Email);


         if (user == null)
         {
            throw new ValidationException("User not found !");
         }
         await _userManager.AddToRoleAsync(user, "Admin");

         var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, isPersistent: true, lockoutOnFailure: true);

         if (result.IsLockedOut)
         {
            throw new ValidationException($"You have tried to login too many times, please try again in {_userManager.Options.Lockout.DefaultLockoutTimeSpan} minutes");
         }
         if (!result.Succeeded)
         {
            throw new ValidationException($"Wrong Password! You have tried {user.AccessFailedCount} times");
         }

         return user;
      }

      public async Task<string> LogoutService(CancellationToken cancellationToken)
      {
         await _signInManager.SignOutAsync();
         return "Logout Success";
      }
   }
}