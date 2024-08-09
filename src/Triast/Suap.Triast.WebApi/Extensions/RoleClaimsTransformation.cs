using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting.Server;
using System.Security.Claims;
using System.Text.Json.Nodes;
using static System.Net.WebRequestMethods;

namespace Suap.Triast.WebApi.Extensions;

internal sealed class RoleClaimsTransformation(
    IServiceProvider serviceProvider)
    : IClaimsTransformation
{


    /// <summary>
    /// Клеймы из resource_access.isys-suap-dev-client.roles -> roles
    /// </summary>
    /// <param name="principal"></param>
    /// <returns></returns>
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        ClaimsIdentity claimsIdentity = new ClaimsIdentity("AuthenticationTypes.Federation",ClaimTypes.Name,"roles");
        var claimType = "resource_access";

        var defaultRealm = "http://localhost:8080/realms/suap-realm";

        if (principal.HasClaim(claim => claim.Type == claimType))
        {
            var claimVal = principal.Claims.FirstOrDefault(x => x.Type == claimType).Value;

            var node = JsonNode.Parse(claimVal.ToString());

            var jsonObject = node.AsObject();

            string[] roleNames = jsonObject.GetValue<string[]>("isys-suap-dev-client.roles");



            foreach (var roleName in roleNames)
                claimsIdentity.AddClaim(new Claim("roles", roleName, ClaimValueTypes.String, defaultRealm, defaultRealm));
            
        }

        principal.AddIdentity(claimsIdentity);
        return Task.FromResult(principal);
    }

    
}
