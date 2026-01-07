using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.JSInterop;
using NetCoreBlazor_main_remake.Client.Models;

namespace NetCoreBlazor_main_remake.Client.Services
{
    public class TipoRecetaService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;
        private const string TOKEN_KEY = "jwt_token";

        public TipoRecetaService(HttpClient http, IJSRuntime js)
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

        public async Task<List<TipoRecetaDTO>> GetAllAsync()
        {
            await AddAuthHeader();
            var response = await _http.GetAsync("api/TipoReceta");
            if (!response.IsSuccessStatusCode) return new();

            var json = JsonSerializer.Deserialize<JsonElement>(
                await response.Content.ReadAsStringAsync());

            return json.GetProperty("datos")
                .EnumerateArray()
                .Select(x => new TipoRecetaDTO
                {
                    Id = x.GetProperty("id").GetInt32(),
                    Name = x.GetProperty("name").GetString() ?? "",
                    Descriptions = x.GetProperty("descriptions").GetString() ?? ""
                }).ToList();
        }

        public async Task<bool> CreateAsync(TipoRecetaCreateDTO dto)
        {
            await AddAuthHeader();
            var response = await _http.PostAsJsonAsync("api/TipoReceta", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, TipoRecetaCreateDTO dto)
        {
            await AddAuthHeader();
            var response = await _http.PutAsJsonAsync($"api/TipoReceta/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await AddAuthHeader();
            var response = await _http.DeleteAsync($"api/TipoReceta/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
