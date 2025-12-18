using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NetCoreBlazor_main_remake.Client;
using NetCoreBlazor_main_remake.Client.Services;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

// HttpClient con BaseAddress de tu API
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://netcoreapi-main-remake.onrender.com/")
});

// Servicios
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ProtectedRouteService>();


await builder.Build().RunAsync();
