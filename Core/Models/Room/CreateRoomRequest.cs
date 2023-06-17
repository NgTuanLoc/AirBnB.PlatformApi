using System.ComponentModel.DataAnnotations;
using System.IO.Enumeration;
using Microsoft.AspNetCore.Http;

namespace Core.Models.Room
{
   public class CreateRoomRequest
   {
      [Required(ErrorMessage = "{0} can not be empty or null")]
      public string Name { get; set; } = default!;
      public string? HomeType { get; set; }
      public string? RoomType { get; set; }
      [Required(ErrorMessage = "{0} can not be empty or null")]
      public int TotalOccupancy { get; set; }
      [Required(ErrorMessage = "{0} can not be empty or null")]
      public int TotalBedrooms { get; set; }
      [Required(ErrorMessage = "{0} can not be empty or null")]
      public int TotalBathrooms { get; set; }
      public string? Summary { get; set; }
      [Required(ErrorMessage = "{0} can not be empty or null")]
      public string Address { get; set; } = "Unknown Address";
      [Required(ErrorMessage = "{0} can not be empty or null")]
      public bool HasTV { get; set; }
      [Required(ErrorMessage = "{0} can not be empty or null")]
      public bool HasKitchen { get; set; }
      [Required(ErrorMessage = "{0} can not be empty or null")]
      public bool HasAirCon { get; set; }
      [Required(ErrorMessage = "{0} can not be empty or null")]
      public bool HasHeating { get; set; }
      [Required(ErrorMessage = "{0} can not be empty or null")]
      public bool HasInternet { get; set; }
      [Required(ErrorMessage = "{0} can not be empty or null")]
      public float Price { get; set; }
      public DateTime? PublishedAt { get; set; }
      public Guid? OwnerId { get; set; } = default!;
      [Required(ErrorMessage = "{0} can not be empty or null")]
      public float Latitude { get; set; }
      [Required(ErrorMessage = "{0} can not be empty or null")]
      public float Longitude { get; set; }
      public Guid? LocationId { get; set; }
      public List<IFormFile>? ImageList { get; set; }
   }
}