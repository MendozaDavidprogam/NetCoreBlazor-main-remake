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

        public AuthService(HttpClient http, IJSRuntime js)
        {
            _http = http;
            _js = js;
        }

        public async Task<string> LoginAsync(UsuarioLogin login)
        {
            try
            {
                Console.WriteLine("Enviando login a la API...");
                var response = await _http.PostAsJsonAsync("api/Usuario/iniciosesion", login);

                // Mostrar información completa de la respuesta
                Console.WriteLine("StatusCode: " + response.StatusCode);
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Body: " + body);

                if (!response.IsSuccessStatusCode) return null;

                var content = JsonSerializer.Deserialize<JsonElement>(body);
                var token = content.GetProperty("datos").GetProperty("jwt").GetString();

                if (!string.IsNullOrEmpty(token))
                    await _js.InvokeVoidAsync("localStorage.setItem", TOKEN_KEY, token);

                return token;
            }
            catch (Exception ex)
            {
                Console.WriteLine("LoginAsync exception: " + ex);
                return null;
            }
        }

        public async Task<string> RegisterAsync(UsuarioRegister user)
        {
            try
            {
                Console.WriteLine("Enviando registro a la API...");
                var response = await _http.PostAsJsonAsync("api/Usuario/registrarse", user);

                Console.WriteLine("StatusCode: " + response.StatusCode);
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Body: " + body);

                if (!response.IsSuccessStatusCode) return null;

                return "ok";
            }
            catch (Exception ex)
            {
                Console.WriteLine("RegisterAsync exception: " + ex);
                return null;
            }
        }

        public async Task LogoutAsync()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", TOKEN_KEY);
        }

        public async Task<string> GetTokenAsync()
        {
            return await _js.InvokeAsync<string>("localStorage.getItem", TOKEN_KEY);
        }
    }
}
