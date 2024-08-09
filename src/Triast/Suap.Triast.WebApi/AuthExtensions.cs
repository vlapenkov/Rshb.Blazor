using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;

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
                //ValidAudience = config["Oidc:ClientId"],
                ValidateLifetime = true,
                //RoleClaimType = "role",
                //NameClaimType = "name"
        }; 
        });
        return services;
    }
}


