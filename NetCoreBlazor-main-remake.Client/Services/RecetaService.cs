//C:\Users\L\Documents\NetCoreBlazor-main-remake\NetCoreBlazor-main-remake.Client\Services\RecetaService.cs
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.JSInterop;
using NetCoreBlazor_main_remake.Client.Models;

namespace NetCoreBlazor_main_remake.Client.Services
{
    public class RecetaService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;
        private const string TOKEN_KEY = "jwt_token";

        public RecetaService(HttpClient http, IJSRuntime js)
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

        public async Task<List<RecetaDTO>> GetAllAsync()
        {
            await AddAuthHeader();
            var response = await _http.GetAsync("api/Receta");
            if (!response.IsSuccessStatusCode) return new();

            var json = JsonSerializer.Deserialize<JsonElement>(
                await response.Content.ReadAsStringAsync());

            return json.GetProperty("datos")
                .EnumerateArray()
                .Select(x => new RecetaDTO
                {
                    Id = x.GetProperty("id").GetInt32(),
                    Name = x.GetProperty("name").GetString() ?? "",
                    Type = x.GetProperty("type").GetInt32(),
                    Leven = x.GetProperty("leven").GetInt32(),
                    QuantityPersons = x.GetProperty("quantityPersons").GetInt32(),
                    Steps = x.GetProperty("steps").GetString() ?? "",
                    Visibility = x.GetProperty("visibility").GetBoolean()
                }).ToList();
        }

        public async Task<bool> CreateAsync(RecetaCreateDTO dto)
        {
            await AddAuthHeader();
            var response = await _http.PostAsJsonAsync("api/Receta", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, RecetaCreateDTO dto)
        {
            await AddAuthHeader();
            var response = await _http.PutAsJsonAsync($"api/Receta/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await AddAuthHeader();
            var response = await _http.DeleteAsync($"api/Receta/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
