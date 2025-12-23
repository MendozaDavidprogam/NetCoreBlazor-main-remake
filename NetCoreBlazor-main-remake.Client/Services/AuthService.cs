// NetCoreBlazor-main-remake.Client/Services/AuthService.cs

using System.Net.Http.Json;
using System.Text.Json;
using NetCoreBlazor_main_remake.Client.Models;
using Microsoft.JSInterop;
using System.Text;

namespace NetCoreBlazor_main_remake.Client.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;

        private const string TOKEN_KEY = "jwt_token";
        private const string USER_ID_KEY = "user_id";

        public event Func<Task>? OnChange;

        private async Task NotifyStateChanged() =>
            await (OnChange?.Invoke() ?? Task.CompletedTask);

        public AuthService(HttpClient http, IJSRuntime js)
        {
            _http = http;
            _js = js;
        }

        // ====================== LOGIN ======================
        public async Task<string?> LoginAsync(UsuarioLogin login)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/Usuario/iniciosesion", login);
                var body = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return null;

                var json = JsonSerializer.Deserialize<JsonElement>(body);
                var token = json.GetProperty("datos").GetProperty("jwt").GetString();
                var userId = json.GetProperty("datos").GetProperty("idusuariosesion").GetInt32();

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

        // ====================== REGISTER ======================
        public async Task<string?> RegisterAsync(UsuarioRegister user)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/Usuario/registrarse", user);
                if (!response.IsSuccessStatusCode)
                    return "Error al registrarse";

                return "ok";
            }
            catch
            {
                return "Error de conexión";
            }
        }

        // ====================== LOGOUT ======================
        public async Task LogoutAsync()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", TOKEN_KEY);
            await _js.InvokeVoidAsync("localStorage.removeItem", USER_ID_KEY);
            await NotifyStateChanged();
        }

        // ====================== TOKEN ======================
        public async Task<string?> GetTokenAsync()
        {
            var token = await _js.InvokeAsync<string>("localStorage.getItem", TOKEN_KEY);

            if (!string.IsNullOrEmpty(token) && IsTokenExpired(token))
            {
                await LogoutAsync();
                return null;
            }

            return token;
        }

        public async Task<bool> IsLoggedInAsync()
        {
            var token = await GetTokenAsync();
            return !string.IsNullOrEmpty(token);
        }

        // ====================== ROLE ======================
        public async Task<string?> GetRoleAsync()
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token)) return null;

            try
            {
                var parts = token.Split('.');
                if (parts.Length != 3) return null;

                var payload = parts[1];
                payload = payload.Replace('-', '+').Replace('_', '/');
                switch (payload.Length % 4)
                {
                    case 2: payload += "=="; break;
                    case 3: payload += "="; break;
                }

                var bytes = Convert.FromBase64String(payload);
                var json = Encoding.UTF8.GetString(bytes);
                var doc = JsonDocument.Parse(json);

                // Claim estándar de rol
                if (doc.RootElement.TryGetProperty("role", out var role))
                    return role.GetString();

                // Claim alternativo (ASP.NET)
                if (doc.RootElement.TryGetProperty("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", out role))
                    return role.GetString();
            }
            catch { }

            return null;
        }

        // ====================== EXP ======================
        private bool IsTokenExpired(string token)
        {
            try
            {
                var parts = token.Split('.');
                if (parts.Length != 3) return true;

                var payload = parts[1];
                payload = payload.Replace('-', '+').Replace('_', '/');
                switch (payload.Length % 4)
                {
                    case 2: payload += "=="; break;
                    case 3: payload += "="; break;
                }

                var bytes = Convert.FromBase64String(payload);
                var json = Encoding.UTF8.GetString(bytes);
                var doc = JsonDocument.Parse(json);

                if (doc.RootElement.TryGetProperty("exp", out var exp))
                {
                    var date = DateTimeOffset.FromUnixTimeSeconds(exp.GetInt64()).UtcDateTime;
                    return date < DateTime.UtcNow;
                }
            }
            catch
            {
                return true;
            }

            return true;
        }
    }
}