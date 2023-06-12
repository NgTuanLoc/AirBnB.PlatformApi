namespace Core.Domain.Entities
{
   public class Location : BaseModel
   {
      public string Name { get; set; } = "Unknown Name";
      public string Province { get; set; } = "Unknown Province";
      public string Country { get; set; } = "Unknown Country";
      public string? Image { get; set; }
      public ICollection<Room>? RoomList { get; set; }
   }
}