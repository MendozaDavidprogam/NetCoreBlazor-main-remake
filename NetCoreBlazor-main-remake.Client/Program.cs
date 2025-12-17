// NetCoreBlazor-main-remake.Client/Program.cs

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NetCoreBlazor_main_remake.Client;
using NetCoreBlazor_main_remake.Client.Services;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

// Configura HttpClient con BaseAddress de tu API
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://netcoreapi-main-remake.onrender.com/")
});

// Registra el servicio de autenticación
builder.Services.AddScoped<AuthService>();

await builder.Build().RunAsync();
