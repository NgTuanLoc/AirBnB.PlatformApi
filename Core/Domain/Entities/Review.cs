using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Domain.Entities
{
   public class Review : BaseModel
   {
      public Reservation? Reservation { get; set; }
      public int Rating { get; set; }
      public string Comment { get; set; } = "No Comment";
      public string Title { get; set; } = "No Title";
   }
}