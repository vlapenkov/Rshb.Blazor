using System.ComponentModel;

[DisplayName("Ошибка сервера")]
public class ServerError : BaseError
{
    public ServerError(string error)
    {
        Error = error;
    }
}