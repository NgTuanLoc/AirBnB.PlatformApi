using Core.Exceptions;
using Core.Models.Auth;
using Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Core.Constants;
using Core.Models.Role;
using Core.Models.User;
using AutoMapper;

namespace Core.Services
{
   public interface IAuthService
   {
      Task<string> RegisterService(RegisterRequest request, CancellationToken cancellationToken);
      Task<CreateUserResponse> LoginService(LoginRequest request, CancellationToken cancellationToken);
      Task<string> LogoutService(CancellationToken cancellationToken);
   }
   public class AuthService : IAuthService
   {
      private readonly UserManager<ApplicationUser> _userManager;
      private readonly SignInManager<ApplicationUser> _signInManager;
      private readonly IRoleService _roleService;
      private readonly IMapper _mapper;
      public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManger, IRoleService roleService, IMapper mapper)
      {
         _userManager = userManager;
         _signInManager = signInManger;
         _roleService = roleService;
         _mapper = mapper;
      }
      public async Task<string> RegisterService(RegisterRequest request, CancellationToken cancellationToken)
      {
         ApplicationUser user = new() { Email = request.Email, PhoneNumber = request.Phone, UserName = request.Email, PersonName = request.PersonName };

         var numberOfUsers = _userManager.Users.Count();
         var isAdminUser = false;

         if (numberOfUsers == 0)
         {
            // Prepare Role List
            var roleList = new List<CreateRoleRequest>()
            {
               new()
               {
                  RoleName = UserRoleOptions.ADMIN
               },
               new()
               {
                  RoleName = UserRoleOptions.USER
               },
               new()
               {
                  RoleName = UserRoleOptions.OWNER
               },

            };
            // Create Role List
            await _roleService.CreateRoleListService(roleList, cancellationToken);
            isAdminUser = true;
         }

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

         // Assign Role and Auto Sign-In
         await _userManager.AddToRoleAsync(user, isAdminUser ? UserRoleOptions.ADMIN : UserRoleOptions.USER);
         await _signInManager.SignInAsync(user, isPersistent: true);
         return "register succeed";
      }
      public async Task<CreateUserResponse> LoginService(LoginRequest request, CancellationToken cancellationToken)
      {
         var user = await _userManager.FindByNameAsync(request.Email) ?? throw new ValidationException("User not found !");
         var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, isPersistent: true, lockoutOnFailure: true);

         if (result.IsLockedOut)
         {
            throw new ValidationException($"You have tried to login too many times, please try again in {_userManager.Options.Lockout.DefaultLockoutTimeSpan} minutes");
         }
         if (!result.Succeeded)
         {
            throw new ValidationException($"Wrong Password! You have tried {user.AccessFailedCount} times");
         }

         CreateUserResponse response = _mapper.Map<ApplicationUser, CreateUserResponse>(user);
         var roleList = await _userManager.GetRolesAsync(user);
         response.RoleList = roleList;

         return response;
      }

      public async Task<string> LogoutService(CancellationToken cancellationToken)
      {
         await _signInManager.SignOutAsync();
         return "Logout Success";
      }
   }
}