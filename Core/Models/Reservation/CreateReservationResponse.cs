namespace Core.Models.Reservation
{
   public class CreateReservationResponse
   {
      public Guid Id { get; set; }
      public string CustomerEmail { get; set; } = default!;
      public string RoomName { get; set; } = default!;
      public DateTime StartDate { get; set; }
      public DateTime EndDate { get; set; }
      public float TotalPrice { get; set; }
      public float TotalGuest { get; set; }
   }
}