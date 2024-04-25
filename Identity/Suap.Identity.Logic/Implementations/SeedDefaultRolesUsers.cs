using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Suap.Identity.Domain;


namespace Suap.IdentityService.Services;

public class SeedDefaultRolesUsers
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
        if (!_roleManager.Roles.Any())
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                var role = new AppRole
                {
                    Name = "Admin"
                };

                await _roleManager.CreateAsync(role);
            }

            if (!await _roleManager.RoleExistsAsync("Viewer"))
            {
                var role = new AppRole
                {
                    Name = "Viewer"
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
            string adminEmail = _config["Authentication:AdminEmail"];
            string adminPassword = _config["Authentication:AdminPassword"];

            var adminUser = new AppUser
            {
                
                Email = adminEmail,
                UserName = adminEmail               
                
            };

            var adminResult = await _userManager.CreateAsync(adminUser, adminPassword);

            if (adminResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, "Admin");
                await _userManager.AddToRoleAsync(adminUser, "Viewer");
            }

            // create a viewer user
            string viewerEmail = _config["Authentication:UserEmail"];
            string viewerPassword = _config["Authentication:UserPassword"];

            var viewerUser = new AppUser
            {
                
                Email = viewerEmail,
                UserName = viewerEmail,
                
            };

            var viewerResult = await _userManager.CreateAsync(viewerUser, viewerPassword);

            if (viewerResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(viewerUser, "viewer");
            }

            
        }
    }
}
