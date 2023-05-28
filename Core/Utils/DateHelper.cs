namespace Core.Utils
{
   public static class DateHelper
   {
      public static string GetDateTimeNowString()
      {
         var dateTimeNowInSecond = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
         var dateTimeNowInSecondToString = dateTimeNowInSecond.ToString().Replace(".", "");
         return dateTimeNowInSecondToString;
      }
   }
}