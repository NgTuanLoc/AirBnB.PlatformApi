namespace Core.Models.Image
{
   public class CreateImageResponse
   {
      public string Title { get; set; } = default!;
      public string Description { get; set; } = default!;
      public string? LowQualityUrl { get; set; }
      public string? MediumQualityUrl { get; set; }
      public string? HighQualityUrl { get; set; }
   }
}