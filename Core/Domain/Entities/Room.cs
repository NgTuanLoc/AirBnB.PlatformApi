using Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity;

namespace Core.Domain.Entities
{
   public class Room : BaseModel
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
      public ApplicationUser Owner { get; set; } = default!;
      public float Latitude { get; set; }
      public float Longitude { get; set; }
      public Location? Location { get; set; }
      public ICollection<Image>? ImageList { get; set; }
      virtual public ICollection<Reservation>? ReservationList { get; set; }
   }
}