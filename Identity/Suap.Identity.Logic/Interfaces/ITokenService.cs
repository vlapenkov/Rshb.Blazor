using Suap.Identity.Domain;

namespace Suap.IdentityService.Services;

public interface ITokenService
{
    Task<string> CreateToken(AppUser user);
    Task<bool> ValidateJwtToken(string token);
}
