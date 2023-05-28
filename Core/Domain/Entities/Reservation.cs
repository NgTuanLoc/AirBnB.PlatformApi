using Core.Domain.IdentityEntities;

namespace Core.Domain.Entities
{
   public class Reservation : BaseModel
   {
      public ApplicationUser? User { get; set; }
      public Room? Room { get; set; }
      public DateTime StartDate { get; set; }
      public DateTime EndDate { get; set; }
      public float Price { get; set; }
      public float Total { get; set; }
   }
}