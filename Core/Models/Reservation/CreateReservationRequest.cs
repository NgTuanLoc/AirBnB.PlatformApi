using System.ComponentModel.DataAnnotations;
using ModelValidationsExample.CustomValidators;

namespace Core.Models.Reservation
{
   public class CreateReservationRequest
   {
      [Required(ErrorMessage = "{0} can not be empty or null")]
      public Guid? UserId { get; set; }
      [Required(ErrorMessage = "{0} can not be empty or null")]
      public Guid? RoomId { get; set; }
      [Required(ErrorMessage = "{0} can not be empty or null")]
      public DateTime StartDate { get; set; }
      [Required(ErrorMessage = "{0} can not be empty or null")]
      [DateRangeValidator("StartDate", ErrorMessage = "'StartDate' should smaller than 'EndDate'")]
      public DateTime EndDate { get; set; }
      [Required(ErrorMessage = "{0} can not be empty or null")]
      [Range(1, 100, ErrorMessage = "{0} can not be empty or null")]
      public float ToTalGuests { get; set; }
   }
   public class DateRange
   {
      public DateTime StartDate { get; set; }
      public DateTime EndDate { get; set; }
   }
}