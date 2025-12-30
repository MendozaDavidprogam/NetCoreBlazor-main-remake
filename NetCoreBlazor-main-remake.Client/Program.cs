// NetCoreBlazor-main-remake.Client/Program.cs
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
    BaseAddress = new Uri("http://localhost:5189/")
});


// Servicios
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ProtectedRouteService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<TipoIngredienteService>();
builder.Services.AddScoped<TipoRecetaService>();
builder.Services.AddScoped<UsuarioAdminService>();
builder.Services.AddScoped<UnidadMedicionService>();
builder.Services.AddScoped<PasoService>();

await builder.Build().RunAsync();
