using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using My.Auth;
using My.LightAuthorizationService.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace My.LightAuthorizationService.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AccountController
    {



        [HttpPost]
        public async Task<IdentResponse<string>> Login([FromBody] UserLogin userLogin)
        {

            ClaimsIdentity identity = GetClaimsIdentity(userLogin);

            return new IdentResponse<string> { Data = GetJwtToken(identity) };

        }

        private ClaimsIdentity GetClaimsIdentity(UserLogin user)
        {
            // Here we can save some values to token.
            // For example we are storing here user id and email
            Claim[] claims = new[]
            {
        new Claim(ClaimTypes.Name, user.UserName),
           //new Claim(ClaimTypes.Role, "Admin"),
           // new Claim(ClaimTypes.Role, "Viewer")

        };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token");

            string[] userRoles = ["Admin", "Viewer", "TestRole"];

            //foreach (var roleName in userRoles)
            //    claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, roleName));

            //claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "TestRole"));

            // Adding roles code
            // Roles property is string collection but you can modify Select code if it it's not
            //claimsIdentity.AddClaims(user.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

            claimsIdentity.AddClaims(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));
            return claimsIdentity;
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
    }
}
