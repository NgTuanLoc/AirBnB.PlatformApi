namespace Common.Exceptions
{
   public class ErrorResponse
   {
      public int ErrorCode { get; set; }
      public string Message { get; set; } = "Something went wrong";
   }
}