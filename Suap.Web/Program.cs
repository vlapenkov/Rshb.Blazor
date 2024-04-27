using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Suap.Web.Services;
using Suap.Web.Components;
using Suap.Web.StateManagement;
using Microsoft.Extensions.DependencyInjection;




WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var configuration = builder.Configuration; 

builder.Services.AddHttpClient("Suap.IdentityService", c => c.BaseAddress = new Uri(configuration["IDENTITY_URI"]!));

builder.Services.AddHttpClient("Suap.Triast", c => c.BaseAddress = new Uri(configuration["TRIAST_URI"]!));

builder.Services.AddSingleton<AppState>();

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddAntDesign();

builder.Services.AddScoped<IIdentityService, IdentityService>();

builder.Services.AddScoped<IApiService, ApiService>();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();


builder.Services.AddAuthorizationCore(options=>options.AddPolicy("AdminPolicy", policy=> policy.RequireRole("Admin")));



builder.Services.AddCascadingAuthenticationState();

var app= builder.Build();

await app.RunAsync();

