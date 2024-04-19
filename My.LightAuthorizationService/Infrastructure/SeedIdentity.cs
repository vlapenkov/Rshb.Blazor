using Microsoft.AspNetCore.Identity;
using My.LightAuthorizationService.Entities;

namespace My.LightAuthorizationService.Services;

public class SeedIdentity
{
    private IConfiguration _config;

    public async Task SeedUsersAndRolesAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config)
    {
        _config = config;
        await SeedRolesAsync(roleManager);
        await SeedUsersAsync(userManager);
    }

    public async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        if (!roleManager.Roles.Any())
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                var role = new IdentityRole
                {
                    Name = "Admin"
                };

                await roleManager.CreateAsync(role);
            }

            if (!await roleManager.RoleExistsAsync("Viewer"))
            {
                var role = new IdentityRole
                {
                    Name = "Viewer"
                };

                await roleManager.CreateAsync(role);
            }
           
        }
    }

    public async Task SeedUsersAsync(UserManager<AppUser> userManager)
    {
        if (!userManager.Users.Any())
        {
            // create an admin user
            string adminEmail = _config["Authentication:AdminEmail"];
            string adminPassword = _config["Authentication:AdminPassword"];

            var adminUser = new AppUser
            {
                DisplayName = "Admin",
                Email = adminEmail,
                UserName = adminEmail               
                
            };

            var adminResult = await userManager.CreateAsync(adminUser, adminPassword);

            if (adminResult.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
                await userManager.AddToRoleAsync(adminUser, "Viewer");
            }

            // create a viewer user
            string viewerEmail = _config["Authentication:UserEmail"];
            string viewerPassword = _config["Authentication:UserPassword"];

            var viewerUser = new AppUser
            {
                DisplayName = "User",
                Email = viewerEmail,
                UserName = viewerEmail,
                
            };

            var viewerResult = await userManager.CreateAsync(viewerUser, viewerPassword);

            if (viewerResult.Succeeded)
            {
                await userManager.AddToRoleAsync(viewerUser, "viewer");
            }

            
        }
    }
}
