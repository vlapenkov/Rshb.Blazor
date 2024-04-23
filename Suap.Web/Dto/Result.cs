using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Suap.Web.Dto;

public class Result<T>
{
    public Result(string[] errorMessages)
    {
        ErrorMessages = errorMessages ?? throw new ArgumentNullException(nameof(errorMessages));
    }
    public Result(T data)
    {
        Data = data ?? throw new ArgumentNullException(nameof(data));
    }

    public T? Data { get; init; }

    public bool IsSuccess => !ErrorMessages.Any();

    public string[] ErrorMessages { get; init; } = new string[0];
}
