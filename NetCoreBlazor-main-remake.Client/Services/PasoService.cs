using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.JSInterop;
using NetCoreBlazor_main_remake.Client.Models;

namespace NetCoreBlazor_main_remake.Client.Services
{
    public class PasoService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;
        private const string TOKEN_KEY = "jwt_token";

        public PasoService(HttpClient http, IJSRuntime js)
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

        public async Task<List<PasoDTO>> GetAllAsync()
        {
            await AddAuthHeader();
            var response = await _http.GetAsync("api/Paso");
            if (!response.IsSuccessStatusCode) return new();

            var json = JsonSerializer.Deserialize<JsonElement>(
                await response.Content.ReadAsStringAsync());

            return json.GetProperty("datos")
                .EnumerateArray()
                .Select(x => new PasoDTO
                {
                    Id = x.GetProperty("id").GetInt32(),
                    Descripcion = x.GetProperty("descripcion").GetString() ?? ""
                }).ToList();
        }

        public async Task<bool> CreateAsync(PasoCreateDTO dto)
        {
            await AddAuthHeader();
            var response = await _http.PostAsJsonAsync("api/Paso", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, PasoCreateDTO dto)
        {
            await AddAuthHeader();
            var response = await _http.PutAsJsonAsync($"api/Paso/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await AddAuthHeader();
            var response = await _http.DeleteAsync($"api/Paso/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
