using Core.Domain.Entities;
using Core.Models.Location;
using Core.Models.Room;

namespace Core.Utils
{
   public static class ConvertEntityIntoResponse
   {
      public static CreateLocationResponse GetLocationResponse(Location location)
      {
         return new CreateLocationResponse()
         {
            Id = location.Id,
            Name = location.Name,
            Province = location.Province,
            Country = location.Country,
            Image = location.Image,
            CreatedDate = location.CreatedDate,
            CreatedBy = location.CreatedBy,
            ModifiedDate = location.ModifiedDate,
            ModifiedBy = location.ModifiedBy,
         };
      }
      public static CreateRoomResponse GetRoomResponse(Room room)
      {
         return new CreateRoomResponse()
         {
            Id = Guid.NewGuid(),
            Name = room.Name,
            HomeType = room.HomeType,
            RoomType = room.RoomType,
            TotalOccupancy = room.TotalOccupancy,
            TotalBedrooms = room.TotalBedrooms,
            TotalBathrooms = room.TotalBathrooms,
            Summary = room.Summary,
            Address = room.Address,
            HasTV = room.HasTV,
            HasKitchen = room.HasKitchen,
            HasAirCon = room.HasAirCon,
            HasHeating = room.HasHeating,
            HasInternet = room.HasInternet,
            Price = room.Price,
            Latitude = room.Latitude,
            Longitude = room.Longitude,
            Owner = room.Owner,
            Location = room.Location != null ? ConvertEntityIntoResponse.GetLocationResponse(room.Location) : null,
            CreatedBy = room.CreatedBy,
            CreatedDate = room.CreatedDate,
         };
      }
   }
}