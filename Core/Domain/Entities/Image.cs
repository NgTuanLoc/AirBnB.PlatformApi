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
      public string? LowQualityUrl { get; set; }
      public string? MediumQualityUrl { get; set; }
      public string? HighQualityUrl { get; set; }
      public Room? Room { get; set; }
   }
}