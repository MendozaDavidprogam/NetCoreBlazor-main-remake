// NetCoreBlazor-main-remake.Client/Services/UsuarioService.cs

using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using NetCoreBlazor_main_remake.Client.Models;
using Microsoft.JSInterop;

namespace NetCoreBlazor_main_remake.Client.Services
{
    public class UsuarioService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;
        private const string TOKEN_KEY = "jwt_token";

        public UsuarioService(HttpClient http, IJSRuntime js)
        {
            _http = http;
            _js = js;
        }

        private async Task AddAuthorizationHeader()
        {
            var token = await _js.InvokeAsync<string>("localStorage.getItem", TOKEN_KEY);
            if (!string.IsNullOrEmpty(token))
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<UsuarioDTO?> GetUsuarioAsync(int id)
        {
            if (id <= 0) return null; // evita llamadas con id=0

            await AddAuthorizationHeader();
            var response = await _http.GetAsync($"api/Usuario/{id}");
            if (!response.IsSuccessStatusCode) return null;

            var body = await response.Content.ReadAsStringAsync();
            var json = JsonSerializer.Deserialize<JsonElement>(body);
            var datos = json.GetProperty("datos");

            return new UsuarioDTO
            {
                Id = datos.GetProperty("id").GetInt32(),
                Name = datos.GetProperty("name").GetString() ?? "",
                Lastname = datos.GetProperty("lastname").GetString() ?? "",
                Email = datos.GetProperty("email").GetString() ?? "",
                Role = datos.GetProperty("role").GetString() ?? "",
                Status = datos.GetProperty("status").GetString() ?? ""
            };
        }

        public async Task<bool> ActualizarUsuarioAsync(int id, string name, string lastname)
        {
            if (id <= 0) return false;

            await AddAuthorizationHeader();

            var payload = new
            {
                Name = name,
                Lastname = lastname
            };

            var response = await _http.PutAsJsonAsync($"api/Usuario/{id}", payload);
            return response.IsSuccessStatusCode;
        }
    }
}
