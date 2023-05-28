using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Core.Domain.IdentityEntities
{
   public class ApplicationUser : IdentityUser<Guid>
   {
      public string PersonName { get; set; } = "Unknown";
      public string Address { get; set; } = "Unknown Address";
      public string ProfileImage { get; set; } = "Unknown ProfileImage";
      public string Description { get; set; } = "Unknown Description";
      public bool? IsMarried { get; set; }
   }
}