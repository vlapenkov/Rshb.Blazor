using Microsoft.EntityFrameworkCore;
using Suap.Common.Api;
using Suap.Identity.Persistence.Extensions;
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

    builder.Services.AddTransient<ITokenService, TokenService>();
});