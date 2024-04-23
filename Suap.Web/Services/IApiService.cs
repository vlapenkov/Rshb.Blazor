
using Suap.Web.Dto;

namespace Suap.Web.Services;

public interface IApiService
{
    Task<Result<T>> GetResult<T>(string clientName, string uri);
}