using AutoMapper;
using Contracts;
using Core.Domain.Entities;
using Core.Domain.IdentityEntities;
using Core.Models.Image;
using Core.Models.Location;
using Core.Models.Reservation;
using Core.Models.Review;
using Core.Models.Role;
using Core.Models.Room;
using Core.Models.User;
using ImageEntity = Core.Domain.Entities.Image;

namespace Core.Utils
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Location, CreateLocationResponse>();
            CreateMap<ImageEntity, CreateImageResponse>();
            CreateMap<Room, CreateRoomResponse>();
            CreateMap<ApplicationRole, CreateRoleResponse>();
            CreateMap<ApplicationUser, CreateUserResponse>();
            CreateMap<Review, CreateReviewResponse>();
            CreateMap<Reservation, CreateReservationResponse>();

            // ASB Mapper
            CreateMap<Room, RoomMessageModel>()
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.Owner.Email))
                .ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => src.Location != null ? src.Location.Name : null));
        }
    }
}