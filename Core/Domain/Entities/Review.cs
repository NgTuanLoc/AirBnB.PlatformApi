using Core.Domain.IdentityEntities;

namespace Core.Domain.Entities
{
   public class Review : BaseModel
   {
      public Reservation Reservation { get; set; } = default!;
      public ApplicationUser User { get; set; } = default!;
      public int Rating { get; set; }
      public string Comment { get; set; } = "No Comment";
      public string Title { get; set; } = "No Title";
   }
}