using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.JSInterop;
using NetCoreBlazor_main_remake.Client.Models;

namespace NetCoreBlazor_main_remake.Client.Services
{
    public class IngredienteService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;
        private const string TOKEN_KEY = "jwt_token";

        public IngredienteService(HttpClient http, IJSRuntime js)
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

        public async Task<List<IngredienteDTO>> GetAllAsync()
        {
            await AddAuthHeader();

            var response = await _http.GetAsync("api/Ingrediente");
            if (!response.IsSuccessStatusCode) return new();

            var json = JsonSerializer.Deserialize<JsonElement>(
                await response.Content.ReadAsStringAsync());

            var datos = json.GetProperty("datos");

            return datos.EnumerateArray().Select(x => new IngredienteDTO
            {
                Id = x.GetProperty("id").GetInt32(),
                Name = x.GetProperty("name").GetString() ?? "",
                Descriptions = x.GetProperty("descriptions").GetString() ?? "",
                TipoIngredienteId = x.GetProperty("tipoIngredienteId").GetInt32(),
                TipoIngredienteNombre = x.GetProperty("tipoIngredienteNombre").GetString() ?? ""
            }).ToList();
        }

        public async Task<bool> CreateAsync(IngredienteCreateDTO dto)
        {
            await AddAuthHeader();
            var response = await _http.PostAsJsonAsync("api/Ingrediente", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, IngredienteCreateDTO dto)
        {
            await AddAuthHeader();
            var response = await _http.PutAsJsonAsync($"api/Ingrediente/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await AddAuthHeader();
            var response = await _http.DeleteAsync($"api/Ingrediente/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
