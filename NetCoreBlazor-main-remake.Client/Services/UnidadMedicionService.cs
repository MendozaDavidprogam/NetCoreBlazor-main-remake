using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.JSInterop;
using NetCoreBlazor_main_remake.Client.Models;

namespace NetCoreBlazor_main_remake.Client.Services
{
    public class UnidadMedicionService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;
        private const string TOKEN_KEY = "jwt_token";

        public UnidadMedicionService(HttpClient http, IJSRuntime js)
        {
            _http = http;
            _js = js;
        }

        private async Task AddAuthHeader()
        {
            var token = await _js.InvokeAsync<string>("localStorage.getItem", TOKEN_KEY);
            if (!string.IsNullOrEmpty(token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<List<UnidadMedicionDTO>> GetAllAsync()
        {
            await AddAuthHeader();
            var response = await _http.GetAsync("api/UnidadMedicion");
            if (!response.IsSuccessStatusCode) return new();

            var json = JsonSerializer.Deserialize<JsonElement>(
                await response.Content.ReadAsStringAsync());

            return json.GetProperty("datos")
                .EnumerateArray()
                .Select(x => new UnidadMedicionDTO
                {
                    Id = x.GetProperty("id").GetInt32(),
                    Name = x.GetProperty("name").GetString() ?? ""
                }).ToList();
        }

        public async Task<bool> CreateAsync(UnidadMedicionCreateDTO dto)
        {
            await AddAuthHeader();
            var response = await _http.PostAsJsonAsync("api/UnidadMedicion", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, UnidadMedicionCreateDTO dto)
        {
            await AddAuthHeader();
            var response = await _http.PutAsJsonAsync($"api/UnidadMedicion/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await AddAuthHeader();
            var response = await _http.DeleteAsync($"api/UnidadMedicion/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
