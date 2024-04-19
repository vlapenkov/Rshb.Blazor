using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using My.Auth;
using My.LightAuthorizationService.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace My.LightAuthorizationService.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly SymmetricSecurityKey _key;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<TokenService> _logger;

    public TokenService(IConfiguration config, UserManager<AppUser> userManager, ILogger<TokenService> logger)
    {
        _userManager = userManager;
        _logger = logger;
        _config = config;
      
    }

    public async Task<string> CreateToken(AppUser user)
    {
        var claims = new List<Claim>
        {
           new Claim(ClaimTypes.Name, user.UserName)
        };

        var roles = await _userManager.GetRolesAsync(user);

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));


        return GetJwtToken(new ClaimsIdentity(claims));
       
    }

    private string GetJwtToken(ClaimsIdentity identity)
    {

        JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
            issuer: AuthJwtTokenOptions.Issuer,
            audience: AuthJwtTokenOptions.Audience,
            notBefore: DateTime.UtcNow,
            claims: identity.Claims,
            // our token will live 1 hour, but you can change you token lifetime here
            expires: DateTime.UtcNow.Add(TimeSpan.FromHours(1)),
            signingCredentials: new SigningCredentials(AuthJwtTokenOptions.GetSecurityKey(), SecurityAlgorithms.HmacSha256));
        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }

   

    

    //TODO: Реализовать

    public async Task<bool> ValidateJwtToken(string token)
    {
        throw new NotImplementedException();


        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidIssuer = _config["JwtIssuer"],
                ValidAudience = _config["JwtAudience"],
                IssuerSigningKey = _key,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                // set clockskew to zero so tokens expire exactly at token expiration time
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var email = jwtToken.Claims.First(x => x.Type == "email").Value;

            // additional checks against user database:

            // does user with this email address exist?
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return false;

            // check if user is locked out
            var userLockedOut = await _userManager.IsLockedOutAsync(user);
            if (userLockedOut) return false;

            // return true if validation and additional checks are successful
            return true;
        }
        catch (SecurityTokenException ex)
        {
            _logger.LogError(ex, "Token Validation Exception: {message}", ex.Message);
            return false;
        }
        catch
        {
            // return false if validation fails
            return false;
        }
    }
}
