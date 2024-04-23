using Suap.Web.Dto;

namespace Suap.Web.Services
{
    public interface IIdentityService
    {
        Task<IdentResponse<string>> Login(UserLogin userLogin);

        Task Logout();
    }
}