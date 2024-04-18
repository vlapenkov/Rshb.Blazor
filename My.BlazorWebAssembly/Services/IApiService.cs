
namespace My.BlazorWebAssembly.Services;

public interface IApiService
{
    Task<Result<T>> GetResult<T>(string clientName, string uri);
}