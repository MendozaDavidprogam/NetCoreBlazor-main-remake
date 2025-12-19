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
        private const string USER_ID_KEY = "user_id";

        public event Func<Task>? OnChange;

        private async Task NotifyStateChanged() => await (OnChange?.Invoke() ?? Task.CompletedTask);

        public AuthService(HttpClient http, IJSRuntime js)
        {
            _http = http;
            _js = js;
        }

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
                        if (content.TryGetProperty("mensaje", out var _))
                        {
                            return null;
                        }
                    }
                    catch { }

                    return null;
                }

                var contentBody = JsonSerializer.Deserialize<JsonElement>(body);
                var token = contentBody.GetProperty("datos").GetProperty("jwt").GetString();
                var userId = contentBody.GetProperty("datos").GetProperty("idusuariosesion").GetInt32();

                if (!string.IsNullOrEmpty(token))
                {
                    await _js.InvokeVoidAsync("localStorage.setItem", TOKEN_KEY, token);
                    await _js.InvokeVoidAsync("localStorage.setItem", USER_ID_KEY, userId.ToString());
                    await NotifyStateChanged();
                }

                return token;
            }
            catch
            {
                return null;
            }
        }

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
            await _js.InvokeVoidAsync("localStorage.removeItem", USER_ID_KEY);
            await NotifyStateChanged();
        }

        public async Task<string?> GetTokenAsync()
        {
            return await _js.InvokeAsync<string>("localStorage.getItem", TOKEN_KEY);
        }

        public async Task<int?> GetUserIdAsync()
        {
            var id = await _js.InvokeAsync<string>("localStorage.getItem", USER_ID_KEY);
            if (int.TryParse(id, out var userId)) return userId;
            return null;
        }

        public async Task<bool> IsLoggedInAsync()
        {
            var token = await GetTokenAsync();
            return !string.IsNullOrEmpty(token);
        }
    }
}
