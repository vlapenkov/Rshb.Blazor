
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using My.LightAuthorizationService.Entities;
using System.Reflection.Emit;

namespace My.LightAuthorizationService.Infrastructure;

public class LightIdentityDbContext : IdentityDbContext<AppUser>
{
    public LightIdentityDbContext(DbContextOptions<LightIdentityDbContext> options) : base(options)
    {
    }
        

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("identity");
        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseSnakeCaseNamingConvention();
        
    }
}
