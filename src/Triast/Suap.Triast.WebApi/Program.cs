using Demo.Authentication.Authentication;
using Rk.Messages.Common.Middlewares;
using Suap.Common.Api;
using Suap.Common.Jwt;
using Suap.Triast.Scripts;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.Extensions.Options;
using Rk.Messages.Common.Extensions;
using Microsoft.IdentityModel.Logging;
using Microsoft.AspNetCore.Authentication;
using Suap.Triast.WebApi.Extensions;
//var timer = new Stopwatch();
//timer.Start();

////for (int i = 0; i < 10; i++)
////Console.WriteLine(SqlTextProvider.GetSqlText("script1.sql"));



//Console.WriteLine(SqlTextProvider.GetSqlText("script11.sql"));

//timer.Stop();

//TimeSpan timeTaken = timer.Elapsed;
//string foo = "Time taken: " + timeTaken.Microseconds.ToString();



//Console.WriteLine($"foo {foo}");

var builder = WebApplication.CreateBuilder(args);



builder.RunApi((host, configuration, services) =>
{

    //builder.WebHost.UseUrls("http://*:8080");

    //Console.WriteLine(Environment.GetEnvironmentVariable("Identity_DB"));
    

    builder.Services.AddTransient<IClaimsTransformation, RoleClaimsTransformation>();


    // Наименование схемы аутентификации по умолчанию
    builder.Services.AddAuthentication(AuthSchemas.Jwt)
    .AddScheme<JwtSchemeOptions, JwtSchemeHandler>(AuthSchemas.Jwt, options => { options.IsActive = true; });

    builder.Services.AddJwtAuthentication(builder.Configuration);
     
           
    
    IdentityModelEventSource.ShowPII = true;


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

        

        app.UseMiddleware<LogUserNameMiddleware>();
    });
