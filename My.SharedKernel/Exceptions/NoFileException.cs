namespace Taxi.Core.Exceptions;

public class NoFileException : AppException
{
    public NoFileException(string message) : base(message) { }
}