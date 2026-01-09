using Microsoft.JSInterop;

namespace NetCoreBlazor_main_remake.Client.Services
{
    public class LanguageService
    {
        private readonly IJSRuntime _js;

        public event Action? OnChange;

        public string CurrentLanguage { get; private set; } = "es";

        private const string StorageKey = "app-language";

        private readonly Dictionary<string, Dictionary<string, string>> _translations =
            new()
            {
                ["es"] = new()
                {
                    ["panel"] = "Panel Culinario",
                    ["home"] = "Home",
                    ["perfil"] = "Perfil",
                    ["recetas"] = "Recetas",
                    ["ingredientes"] = "Ingredientes",
                    ["pasos"] = "Pasos",
                    ["tipoIngredientes"] = "Tipo Ingredientes",
                    ["tipoRecetas"] = "Tipo Recetas",
                    ["unidades"] = "Unidades de Medición",
                    ["usuarios"] = "Usuarios Registrados",
                    ["modoOscuro"] = "Modo Oscuro",
                    ["modoClaro"] = "Modo Claro",
                    ["cerrarSesion"] = "Cerrar sesión"
                },
                ["en"] = new()
                {
                    ["panel"] = "Culinary Panel",
                    ["home"] = "Home",
                    ["perfil"] = "Profile",
                    ["recetas"] = "Recipes",
                    ["ingredientes"] = "Ingredients",
                    ["pasos"] = "Steps",
                    ["tipoIngredientes"] = "Ingredient Types",
                    ["tipoRecetas"] = "Recipe Types",
                    ["unidades"] = "Measurement Units",
                    ["usuarios"] = "Registered Users",
                    ["modoOscuro"] = "Dark Mode",
                    ["modoClaro"] = "Light Mode",
                    ["cerrarSesion"] = "Log out"
                }
            };

        public LanguageService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task InitializeAsync()
        {
            var saved = await _js.InvokeAsync<string>("localStorage.getItem", StorageKey);
            if (!string.IsNullOrWhiteSpace(saved))
                CurrentLanguage = saved;
        }

        public async Task ToggleLanguageAsync()
        {
            CurrentLanguage = CurrentLanguage == "es" ? "en" : "es";
            await _js.InvokeVoidAsync("localStorage.setItem", StorageKey, CurrentLanguage);
            OnChange?.Invoke();
        }

        public string Translate(string key)
        {
            if (_translations.TryGetValue(CurrentLanguage, out var lang) &&
                lang.TryGetValue(key, out var value))
            {
                return value;
            }

            return key;
        }
    }
}
