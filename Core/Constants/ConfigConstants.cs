namespace Core.Constants
{
   public static class ConfigConstants
   {
      public const string BLOB_CONTAINER = "airbnbimagestorage";
      public const string SEEDING_BLOB_DATA_CONTAINER = "seedingdatastorage";
      public const int LOCKOUT_TIME_SPAN = 60;
      public const string LOCATION_SEEDER_QUEUE = "location-seeder-queue";
      public const string ROOM_SEEDER_QUEUE = "room-seeder-queue";
      public const string LOCATION_JSON_BLOB_NAME = "Data/Json/location.json";
      public const string ROOM_JSON_BLOB_NAME = "Data/Json/room.json";
      public const int AMOUNT_OF_MESSAGES_PER_BATCH = 10;
   }
}