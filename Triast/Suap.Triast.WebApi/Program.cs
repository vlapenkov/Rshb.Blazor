using Demo.Authentication.Authentication;
using Suap.Common.Api;
using Suap.Common.Jwt;

var builder = WebApplication.CreateBuilder(args);


builder.RunApi((host, configuration, services) =>
{
    builder.Services
                 // Наименование схемы аутентификации по умолчанию
                 .AddAuthentication(AuthSchemas.Jwt)
                 .AddScheme<JwtSchemeOptions, JwtSchemeHandler>(AuthSchemas.Jwt, options => { options.IsActive = true; });

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
        options.AddPolicy("FakePolicy", policy => policy.RequireRole("Fake"));
    });

    builder.Services.AddHttpContextAccessor();
},
    app =>
    {
        app.UseAuthentication();
        app.UseAuthorization();
    });
