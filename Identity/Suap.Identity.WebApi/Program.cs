using FluentValidation.AspNetCore;
using FluentValidation;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Suap.Identity.Persistence.Extensions;
using Suap.IdentityService.Infrastructure;
using Suap.IdentityService.Services;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Serialization;
using System.ComponentModel;
using System.Text.Json;
using Common.Logging;
using Suap.Identity.WebApi.Middlewares;


var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

# region REST_API
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
      //  options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
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

#endregion

builder.Services.AddDbContext<AppIdentityDbContext>(
   //options => options.UseNpgsql(builder.Configuration.GetConnectionString("IdentityConnection"))
   options => options.ConfigDatabase(configuration.GetConnectionString("IdentityConnection")!)
);


builder.Services.AddIdentityServices();



builder.Services.ConfigureOptions();



//builder.Services.AddControllers();



builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin()
        );
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddTransient<SeedDefaultRolesUsers>();

builder.Services.AddTransient<ITokenService, TokenService>();

var app = builder.Build();


//await SeedIdentityData(app);


app.UseCors("CorsPolicy");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ErrorHandlerMiddleware>();

//var seedService = app.Services.GetService<SeedDefaultRolesUsers>();

//await seedService.SeedUsersAndRolesAsync();

app.Run();
