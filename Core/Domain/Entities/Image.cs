using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Domain.Entities
{
   public class Image : BaseModel
   {
      public string Title { get; set; } = "Unknown Title";
      public string Description { get; set; } = "Unknown Description";
      public string? Url { get; set; }
      public Room? Room { get; set; }
   }
}