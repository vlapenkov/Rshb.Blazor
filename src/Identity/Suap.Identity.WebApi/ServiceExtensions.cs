using Suap.Identity.Logic.Implementations;
using Suap.Identity.Logic.Interfaces;
using Suap.IdentityService.Services;

namespace Suap.Identity.WebApi;

public static class ServiceExtensions
{
    public static void AddDependencies(this IServiceCollection services) {

        services.AddScoped<ISeedDefaultRolesUsers, SeedDefaultRolesUsers>();

        services.AddScoped<IAccountService, AccountService>();

        services.AddScoped<IRoleService, RoleService>();

        services.AddScoped<IUserService, UserService>();

        services.AddTransient<ITokenService, TokenService>();

    }
}
