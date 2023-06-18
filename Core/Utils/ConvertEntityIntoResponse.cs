using Core.Domain.Entities;
using Core.Models.Image;
using Core.Models.Location;
using Core.Models.Room;
using ImageEntity = Core.Domain.Entities.Image;

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
         List<CreateImageResponse>? imageList = null;

         if (room.ImageList != null)
         {
            imageList = new List<CreateImageResponse>();
            foreach (var image in room.ImageList)
            {
               imageList.Add(ConvertEntityIntoResponse.GetImageResponse(image));
            }
         }

         return new CreateRoomResponse()
         {
            Id = room.Id,
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
            ImageList = imageList,
            CreatedBy = room.CreatedBy,
            CreatedDate = room.CreatedDate,
         };
      }

      public static CreateImageResponse GetImageResponse(ImageEntity image)
      {
         return new CreateImageResponse()
         {
            Id = image.Id,
            Title = image.Title,
            Description = image.Description,
            HighQualityUrl = image.HighQualityUrl,
            MediumQualityUrl = image.MediumQualityUrl,
            LowQualityUrl = image.LowQualityUrl,
            CreatedDate = image.CreatedDate,
            CreatedBy = image.CreatedBy,
            ModifiedDate = image.ModifiedDate,
            ModifiedBy = image.ModifiedBy,
            RoomId = image?.Room?.Id
         };
      }
   }
}