namespace Core.Models.Review
{
   public class CreateReviewResponse
   {
      public Guid Id { get; set; }
      public string Title { get; set; } = "Unknown Title";
      public string Comment { get; set; } = "Unknown Comment";
      public int Rating { get; set; }
      public string RoomName { get; set; } = "Unknown Room Name";
      public string UserEmail { get; set; } = "Unknown User";
   }
}