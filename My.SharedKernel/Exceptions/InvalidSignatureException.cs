namespace Taxi.Core.Exceptions;

public class InvalidSignatureException : AppException
{
    public InvalidSignatureException(string message) : base(message)
    {
    }
}
