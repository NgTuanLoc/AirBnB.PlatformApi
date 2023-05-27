using Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbContext
{
   public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
   {
      public ApplicationDbContext(DbContextOptions options) : base(options)
      {
      }
   }
}