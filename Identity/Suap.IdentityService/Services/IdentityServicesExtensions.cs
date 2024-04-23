
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using My.LightAuthorizationService.Entities;
using My.LightAuthorizationService.Infrastructure;
using System.Text;

namespace My.LightAuthorizationService.Services;

public static class IdentityServicesExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services.AddIdentityCore<AppUser>()
            .AddRoles<IdentityRole>()
            .AddRoleManager<RoleManager<IdentityRole>>()
            .AddSignInManager<SignInManager<AppUser>>()
            //.AddRoleValidator<RoleValidator<IdentityRole>>()
            .AddEntityFrameworkStores<LightIdentityDbContext>();

        // обязательно, иначе не работает SignInManager
        services.AddAuthentication();          

        return services;
    }

    public static IServiceCollection ConfigureOptions(this IServiceCollection services) {

        services.Configure<IdentityOptions>(options =>
        {
            // Default Password settings.
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 3;
            options.Password.RequiredUniqueChars = 1;
        });

        return services;
    }
}
