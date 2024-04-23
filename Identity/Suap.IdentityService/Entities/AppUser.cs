using Microsoft.AspNetCore.Identity;

namespace My.LightAuthorizationService.Entities;

public class AppUser : IdentityUser
{
    public string DisplayName { get; set; }

}
