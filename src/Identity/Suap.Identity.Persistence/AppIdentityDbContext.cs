
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Suap.Identity.Domain;
using System.Reflection.Emit;


namespace Suap.IdentityService.Infrastructure;

public class AppIdentityDbContext : IdentityDbContext<AppUser, AppRole,Guid>
{
    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder builder)
    {        
        base.OnModelCreating(builder);

        builder.Entity<AppUser>(b =>
        {
            b.ToTable("users");
        });

        builder.Entity<AppRole>(b =>
        {
            b.ToTable("roles");
        });

        builder.Entity<IdentityUserClaim<Guid>>(b =>
        {
            b.ToTable("user_claims");
        });

        //builder.Ignore<IdentityUserLogin<string>>();

        builder.Entity<IdentityUserLogin<Guid>>(b =>
        {
            b.ToTable("user_logins");
            b.HasKey(p=>new { p.LoginProvider, p.ProviderKey });
        });

        builder.Entity<IdentityUserToken<Guid>>(b =>
        {
            b.ToTable("user_tokens");
            b.HasKey(p => new { p.UserId, p.LoginProvider,p.Name});

        });

        builder.Entity<IdentityRoleClaim<Guid>>(b =>
        {
            b.ToTable("role_claims");
        });

        builder.Entity<IdentityUserRole<Guid>>(b =>
        {
            b.ToTable("user_roles");
            b.HasKey(p => new { p.UserId, p.RoleId });
        });


    }


#if false
    public class AppIdentityDbContextFactory : IDesignTimeDbContextFactory<AppIdentityDbContext>
    {
        public AppIdentityDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                    .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AppIdentityDbContext>();
            //optionsBuilder.UseNpgsql(options => options.ConfigDatabase(configuration.GetConnectionString("IdentityConnection")));
            var connectionString = configuration.GetConnectionString("IdentityConnection");

            optionsBuilder.UseNpgsql(connectionString);

            return new AppIdentityDbContext(optionsBuilder.Options);
        }
    }

#endif
}
