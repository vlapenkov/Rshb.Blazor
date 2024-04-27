
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Suap.Identity.Domain;
using Suap.IdentityService.Infrastructure;


namespace Suap.Identity.Persistence.Extensions;

public static class IdentityExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services.AddIdentityCore<AppUser>()
            .AddRoles<AppRole>()            
            .AddSignInManager<SignInManager<AppUser>>()
            .AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider)            
            .AddEntityFrameworkStores<AppIdentityDbContext>();

        // обязательно, иначе не работает SignInManager
        services.AddAuthentication();

        return services;
    }

   

    public static IServiceCollection ConfigureOptions(this IServiceCollection services)
    {

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
