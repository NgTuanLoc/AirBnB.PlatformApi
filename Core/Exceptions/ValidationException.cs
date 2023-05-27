namespace Core.Exceptions
{
   public class ValidationException : Exception
   {
      public IDictionary<string, string[]> Failures { get; } = new Dictionary<string, string[]>();
      public IDictionary<string, object[]> ValidateFailures { get; } = new Dictionary<string, object[]>();
      public ValidationException() : base("One or more validation failures have occurred.") { }
      public ValidationException(string message) : base(message) { }
      public ValidationException(IDictionary<string, string[]> failures) : this() => Failures = failures;
      public ValidationException(IDictionary<string, object[]> failures) : this() => ValidateFailures = failures;
   }
}