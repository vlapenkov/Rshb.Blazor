namespace Suap.Common.Exceptions;

public class SigningMismatchException : AppException
{
    public SigningMismatchException(string message) : base(message)
    {
    }
}
