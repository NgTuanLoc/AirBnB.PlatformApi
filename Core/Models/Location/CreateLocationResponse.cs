using Core.Domain.Entities;

namespace Core.Models.Location
{
   public class CreateLocationResponse : BaseModel
   {
      public string Name { get; set; } = default!;
      public string Province { get; set; } = default!;
      public string Country { get; set; } = default!;
      public string? Image { get; set; } = default!;
   }
}