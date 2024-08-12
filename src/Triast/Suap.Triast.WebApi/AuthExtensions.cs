using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Rk.Messages.Common.Extensions;

public static class AuthExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
    {
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
        {
            o.Authority = config["Oidc:Authority"];
            //o.Audience = config["Oidc:ClientId"];
            //o.MetadataAddress = config["Oidc:Metadata"];
            o.IncludeErrorDetails = true;
            o.RequireHttpsMetadata = false;
            


            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,               
                ValidateIssuer = true,
                ValidIssuer = config["Oidc:Authority"],                
                ValidateLifetime = true,
                RoleClaimType = "roles", // кастомный value, в который перекинем клеймы из keycloak                
        }; 
        });
        return services;
    }
}


