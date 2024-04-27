using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Suap.Common.Api;
using Suap.Identity.Logic.Implementations;
using Suap.Identity.Logic.Interfaces;
using Suap.Identity.Persistence.Extensions;
using Suap.Identity.WebApi;
using Suap.IdentityService.Infrastructure;
using Suap.IdentityService.Services;


var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;



builder.RunApi((host, configuration, services) =>
{
    builder.Services.AddDbContext<AppIdentityDbContext>(
   options => options.ConfigDatabase(configuration.GetConnectionString("IdentityConnection")!)
);

    builder.Services.AddIdentityServices();

    builder.Services.ConfigureOptions();

    builder.Services.AddHttpContextAccessor();

    builder.Services.AddDependencies();


},
   (Action<WebApplication>?)(async  app =>
   {
       await SeedDefaultRolesAndUser(app);

   }));

static async Task SeedDefaultRolesAndUser(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        var seedService = services.GetRequiredService<ISeedDefaultRolesUsers>();
        await seedService.SeedUsersAndRolesAsync();
    }
}