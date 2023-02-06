namespace List_Domain.Exeptions
{
    public class NotFoundException : Exception // ПОГАНОО, один клас, один файл
    {
        public NotFoundException(string message) : base(message) { }

        public NotFoundException() { }
    }

    public class ValidProblemException : Exception
    {
        public ValidProblemException(string message) : base(message) { }

        public ValidProblemException() { }
    }
}
