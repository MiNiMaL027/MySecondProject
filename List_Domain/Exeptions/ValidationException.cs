namespace List_Domain.Exeptions
{
     public class ValidationException : Exception
     {
        public ValidationException(string message) : base(message) { }

        public ValidationException() { }
     }
}
