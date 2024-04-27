using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Suap.Identity.Domain;
using Suap.Identity.Domain.Enums;
using Suap.Identity.Logic.Implementations;
using Suap.Identity.Logic.Interfaces;


namespace Suap.IdentityService.Services;

public class SeedDefaultRolesUsers : ISeedDefaultRolesUsers
{
    private IConfiguration _config;
    private UserManager<AppUser> _userManager;
    private RoleManager<AppRole> _roleManager;

    public SeedDefaultRolesUsers(IConfiguration config, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        _config = config;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedUsersAndRolesAsync()
    {
        await SeedRolesAsync();
        await SeedUsersAsync();
    }


    public async Task SeedRolesAsync()
    {

        var roleNames = Enum.GetValues(typeof(DefaultRoles)).Cast<DefaultRoles>().Select(p => EnumDescriptionProvider.GetDescription(p)).ToArray();

        foreach (var roleName in roleNames)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var role = new AppRole
                {
                    Name = roleName
                };

                await _roleManager.CreateAsync(role);
            }
        }

    }

    public async Task SeedUsersAsync()
    {
        if (!_userManager.Users.Any())
        {
            // create an admin user
            string adminName = _config["Authentication:AdminName"];
            string adminEmail = _config["Authentication:AdminEmail"];
            string adminPassword = _config["Authentication:AdminPassword"];

            var adminUser = new AppUser
            {

                Email = adminEmail,
                UserName = adminName

            };

            var adminResult = await _userManager.CreateAsync(adminUser, adminPassword);

            if (adminResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, EnumDescriptionProvider.GetDescription(DefaultRoles.Admin));
            }


        }
    }
}
