using Suap.Identity.Logic.Dto;

namespace Suap.Identity.Logic.Interfaces;
public interface IAccountService
{
    Task ChangePassword(ChangePasswordRequest model);
    Task<string> Login(LoginRequest model);
}