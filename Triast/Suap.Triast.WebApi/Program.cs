using Common.Logging;
using Demo.Authentication.Authentication;
using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Suap.Common.Jwt;
using Suap.Triast.Converters;
using Suap.Triast.Middlewares;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// для логирования ставит не корневую директорию, а bin\Debug\net8.0\Logs
Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
builder.Host.UseLogging();
// Add services to the container.

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(o =>
    {
        o.InvalidModelStateResponseFactory = c => new BadRequestObjectResult(new ValidationError(c.ModelState));
    })
    .AddJsonOptions(options =>
    {

        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    });



builder.Services.AddSwaggerGen(c =>
{
    c.DescribeAllParametersInCamelCase();
    var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath, true);
});

builder.Services.AddFluentValidationRulesToSwagger();

//ValidatorOptions.Global.LanguageManager = new RussianLanguageManager();
ValidatorOptions.Global.PropertyNameResolver =
    (Type type, MemberInfo memberInfo, LambdaExpression expression) =>
        JsonNamingPolicy.CamelCase.ConvertName(memberInfo?.Name ?? string.Empty);
ValidatorOptions.Global.DisplayNameResolver =
    (Type type, MemberInfo memberInfo, LambdaExpression expression) =>
    {
        var a = memberInfo?.GetCustomAttribute<DisplayNameAttribute>();
        return a != null ? a.DisplayName : memberInfo?.Name;
    };

builder.Services.AddValidatorsFromAssembly(Assembly.GetEntryAssembly());
builder.Services.AddFluentValidationAutoValidation(c => c.DisableDataAnnotationsValidation = true);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
                // Наименование схемы аутентификации по умолчанию
                .AddAuthentication(AuthSchemas.Jwt)                
                .AddScheme<JwtSchemeOptions, JwtSchemeHandler>(AuthSchemas.Jwt, options => { options.IsActive = true; });

builder.Services.AddAuthorization(options => { 
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("FakePolicy", policy => policy.RequireRole("Fake"));
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseCors(builder => builder.AllowAnyOrigin()
                         .AllowAnyHeader()
                         .AllowAnyMethod());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

//app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapControllers();

app.Run();
