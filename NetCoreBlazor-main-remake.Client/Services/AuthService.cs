// NetCoreBlazor-main-remake.Client/Services/AuthService.cs

using System.Net.Http.Json;
using System.Text.Json;
using NetCoreBlazor_main_remake.Client.Models;
using Microsoft.JSInterop;

namespace NetCoreBlazor_main_remake.Client.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;
        private const string TOKEN_KEY = "jwt_token";

        public event Func<Task>? OnChange;

        private async Task NotifyStateChanged() => await (OnChange?.Invoke() ?? Task.CompletedTask);

        public AuthService(HttpClient http, IJSRuntime js)
        {
            _http = http;
            _js = js;
        }

        /*public async Task<string?> LoginAsync(UsuarioLogin login)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/Usuario/iniciosesion", login);
                if (!response.IsSuccessStatusCode) return null;

                var body = await response.Content.ReadAsStringAsync();
                var content = JsonSerializer.Deserialize<JsonElement>(body);
                var token = content.GetProperty("datos").GetProperty("jwt").GetString();

                if (!string.IsNullOrEmpty(token))
                {
                    await _js.InvokeVoidAsync("localStorage.setItem", TOKEN_KEY, token);
                    await NotifyStateChanged();
                }

                return token;
            }
            catch
            {
                return null;
            }
        }*/

        public async Task<string?> LoginAsync(UsuarioLogin login)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/Usuario/iniciosesion", login);
                var body = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    try
                    {
                        var content = JsonSerializer.Deserialize<JsonElement>(body);
                        if (content.TryGetProperty("mensaje", out var mensaje))
                        {
                            return null; // Backend ya envió mensaje, Blazor puede mostrarlo
                        }
                    }
                    catch { }

                    return null;
                }

                var contentBody = JsonSerializer.Deserialize<JsonElement>(body);
                var token = contentBody.GetProperty("datos").GetProperty("jwt").GetString();

                if (!string.IsNullOrEmpty(token))
                {
                    await _js.InvokeVoidAsync("localStorage.setItem", TOKEN_KEY, token);
                    await NotifyStateChanged();
                }

                return token;
            }
            catch
            {
                return null;
            }
        }


        /*public async Task<string?> RegisterAsync(UsuarioRegister user)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/Usuario/registrarse", user);
                if (!response.IsSuccessStatusCode) return null;

                return "ok";
            }
            catch
            {
                return null;
            }
        }*/

        public async Task<string?> RegisterAsync(UsuarioRegister user)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/Usuario/registrarse", user);
                var body = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    try
                    {
                        var content = JsonSerializer.Deserialize<JsonElement>(body);
                        if (content.TryGetProperty("mensaje", out var mensaje))
                        {
                            return mensaje.GetString();
                        }
                    }
                    catch { }

                    return "Error al registrarse";
                }

                return "ok";
            }
            catch
            {
                return "Error de conexión al servidor";
            }
        }


        public async Task LogoutAsync()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", TOKEN_KEY);
            await NotifyStateChanged();
        }

        public async Task<string?> GetTokenAsync()
        {
            return await _js.InvokeAsync<string>("localStorage.getItem", TOKEN_KEY);
        }

        public async Task<bool> IsLoggedInAsync()
        {
            var token = await GetTokenAsync();
            return !string.IsNullOrEmpty(token);
        }
    }
}
