using Microsoft.JSInterop;

namespace NetCoreBlazor_main_remake.Client.Services
{
    public class ThemeService
    {
        private const string THEME_KEY = "app_theme";
        private readonly IJSRuntime _js;

        public event Action? OnChange;

        public string CurrentTheme { get; private set; } = "light";

        public ThemeService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task InitializeAsync()
        {
            var storedTheme = await _js.InvokeAsync<string>("localStorage.getItem", THEME_KEY);
            if (!string.IsNullOrEmpty(storedTheme))
            {
                CurrentTheme = storedTheme;
            }

            await ApplyThemeAsync();
        }

        public async Task ToggleThemeAsync()
        {
            CurrentTheme = CurrentTheme == "light" ? "dark" : "light";
            await _js.InvokeVoidAsync("localStorage.setItem", THEME_KEY, CurrentTheme);
            await ApplyThemeAsync();
            OnChange?.Invoke();
        }

        private async Task ApplyThemeAsync()
        {
            await _js.InvokeVoidAsync("setTheme", CurrentTheme);
        }
    }
}
