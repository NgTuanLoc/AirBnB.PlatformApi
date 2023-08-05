namespace Core.Constants
{
   public class ProcessingLowQualityImageConstants
   {
      // ! Boundary: 0 <= GAUSSIAN_BLUR_SIGMA <= 5
      public const float GAUSSIAN_BLUR_SIGMA = (float)2.5;
      public const int RESIZE_WIDTH = 50;
      public const int RESIZE_HEIGHT = 50;
      public const int IMAGE_QUALITY = 50;
   }
   public class ProcessingMediumQualityImageConstants
   {
      // ! Boundary: 0 <= GAUSSIAN_BLUR_SIGMA <= 5
      public const float GAUSSIAN_BLUR_SIGMA = (float)1.5;
      public const int RESIZE_WIDTH = 400;
      public const int RESIZE_HEIGHT = 4000;
      public const int IMAGE_QUALITY = 70;
   }
}