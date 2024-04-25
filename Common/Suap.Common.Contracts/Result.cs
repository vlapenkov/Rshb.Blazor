namespace Suap.Common.Contracts;

public class Result<T>
{

    public static Result<T> FromSuccess(T data) => new Result<T> { Data = data };

    public static Result<T> FromError(string[] messages) => new Result<T> { ErrorMessages = messages };    

    //Один публичный конструктор для десериализации
    public Result()
    {}

    public T? Data { get; init; }

    public bool IsSuccess => !ErrorMessages.Any();

    public string[] ErrorMessages { get; init; } = new string[0];
}
