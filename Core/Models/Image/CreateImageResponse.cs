using Core.Domain.Entities;

namespace Core.Models.Image
{
   public class CreateImageResponse : BaseModel
   {
      public string Title { get; set; } = default!;
      public string Description { get; set; } = default!;
      public string? LowQualityUrl { get; set; }
      public string? MediumQualityUrl { get; set; }
      public string? HighQualityUrl { get; set; }
      public Guid? RoomId { get; set; }
   }
}