using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Core.Models.Room
{
   public class UpdateRoomRequest
   {
      [Required(ErrorMessage = "{0} can not be empty or null")]
      public string Name { get; set; } = default!;
      public string? HomeType { get; set; }
      public string? RoomType { get; set; }
      public int? TotalOccupancy { get; set; }
      public int? TotalBedrooms { get; set; }
      public int? TotalBathrooms { get; set; }
      public string? Summary { get; set; }
      public string? Address { get; set; }
      public bool? HasTV { get; set; }
      public bool? HasKitchen { get; set; }
      public bool? HasAirCon { get; set; }
      public bool? HasHeating { get; set; }
      public bool? HasInternet { get; set; }
      public float? Price { get; set; }
      public DateTime? PublishedAt { get; set; }
      public Guid? OwnerId { get; set; } = default!;
      public float? Latitude { get; set; }
      public float? Longitude { get; set; }
      public Guid? LocationId { get; set; }
      public List<IFormFile>? ImageList { get; set; }
   }
}