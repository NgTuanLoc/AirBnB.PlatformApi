namespace Core.Domain.Entities
{
   public abstract class BaseModel
   {
      public Guid Id { get; set; }
      public DateTime CreatedDate { get; set; }
      public string CreatedBy { get; set; } = "Unknown";
      public DateTime? ModifiedDate { get; set; }
      public string? ModifiedBy { get; set; }
   }
}