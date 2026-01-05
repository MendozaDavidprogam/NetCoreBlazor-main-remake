// NetCoreBlazor-main-remake.Client/Services/UsuarioAdminService.cs
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using NetCoreBlazor_main_remake.Client.Models;
using Microsoft.JSInterop;

namespace NetCoreBlazor_main_remake.Client.Services
{
    public class UsuarioAdminService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;
        private const string TOKEN_KEY = "jwt_token";

        public UsuarioAdminService(HttpClient http, IJSRuntime js)
        {
            _http = http;
            _js = js;
        }

        private async Task AddAuthorizationHeader()
        {
            var token = await _js.InvokeAsync<string>("localStorage.getItem", TOKEN_KEY);
            if (!string.IsNullOrEmpty(token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<List<UsuarioDTO>> ObtenerUsuariosAsync()
        {
            await AddAuthorizationHeader();

            var response = await _http.GetAsync("api/Usuario");
            if (!response.IsSuccessStatusCode)
                return new List<UsuarioDTO>();

            var body = await response.Content.ReadAsStringAsync();
            var json = JsonSerializer.Deserialize<JsonElement>(body);

            return json.GetProperty("datos")
                .EnumerateArray()
                .Select(u => new UsuarioDTO
                {
                    Id = u.GetProperty("id").GetInt32(),
                    Name = u.GetProperty("name").GetString() ?? "",
                    Lastname = u.GetProperty("lastname").GetString() ?? "",
                    Email = u.GetProperty("email").GetString() ?? "",
                    Role = u.GetProperty("role").GetString() ?? "",
                    Status = u.GetProperty("status").GetString() ?? ""
                })
                .ToList();
        }
    }
}
