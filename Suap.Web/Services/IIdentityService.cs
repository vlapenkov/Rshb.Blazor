using Suap.Identity.Contracts;
using Suap.Web.Dto;

namespace Suap.Web.Services
{
    public interface IIdentityService
    {
        Task<TokenResponse> Login(UserLogin userLogin);

        Task Logout();
    }
}