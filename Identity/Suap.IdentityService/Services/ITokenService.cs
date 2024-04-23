using My.LightAuthorizationService.Entities;

namespace My.LightAuthorizationService.Services;

public interface ITokenService
{
    Task<string> CreateToken(AppUser user);
    Task<bool> ValidateJwtToken(string token);
}
