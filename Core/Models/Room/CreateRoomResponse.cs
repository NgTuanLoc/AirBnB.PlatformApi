using Core.Domain.Entities;
using Core.Domain.IdentityEntities;
using Core.Models.Image;
using Core.Models.Location;

namespace Core.Models.Room
{
   public class CreateRoomResponse : BaseModel
   {
      public string Name { get; set; } = default!;
      public string? HomeType { get; set; }
      public string? RoomType { get; set; }
      public int TotalOccupancy { get; set; }
      public int TotalBedrooms { get; set; }
      public int TotalBathrooms { get; set; }
      public string? Summary { get; set; }
      public string Address { get; set; } = "Unknown Address";
      public bool HasTV { get; set; }
      public bool HasKitchen { get; set; }
      public bool HasAirCon { get; set; }
      public bool HasHeating { get; set; }
      public bool HasInternet { get; set; }
      public float Price { get; set; }
      public DateTime PublishedAt { get; set; }
      public ApplicationUser? Owner { get; set; }
      public float Latitude { get; set; }
      public float Longitude { get; set; }
      public CreateLocationResponse? Location { get; set; }
      public ICollection<CreateImageResponse>? ImageList { get; set; }
   }
}