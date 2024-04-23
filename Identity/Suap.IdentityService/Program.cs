using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using My.LightAuthorizationService.Infrastructure;
using My.LightAuthorizationService.Services;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

builder.Services.AddDbContext<LightIdentityDbContext>(
           options => options.ConfigDatabase(configuration.GetConnectionString("IdentityConnection")!)
);


builder.Services.AddIdentityServices();

builder.Services.ConfigureOptions();

// Add services to the container.

builder.Services.AddControllers();



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
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<SeedIdentity>();
builder.Services.AddTransient<ITokenService, TokenService>();

var app = builder.Build();


//await SeedIdentityData(app);

//app.UseCors(builder => builder.AllowAnyOrigin()
//                         .AllowAnyHeader()
//                         .AllowAnyMethod());


app.UseCors("CorsPolicy");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
