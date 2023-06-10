namespace Core.Models.Image
{
   public class UploadImageResponse
   {
      public string? highQualityUrl { get; set; }
      public string? mediumQualityUrl { get; set; }
      public string? lowQualityUrl { get; set; }
   }
}