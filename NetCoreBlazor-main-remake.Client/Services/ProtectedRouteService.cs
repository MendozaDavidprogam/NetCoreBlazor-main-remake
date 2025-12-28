// NetCoreBlazor-main-remake.Client/Services/ProtectedRouteService.cs
using Microsoft.AspNetCore.Components;

namespace NetCoreBlazor_main_remake.Client.Services
{
    public class ProtectedRouteService
    {
        private readonly NavigationManager _navigation;
        private readonly AuthService _auth;

        public ProtectedRouteService(NavigationManager navigation, AuthService auth)
        {
            _navigation = navigation;
            _auth = auth;
        }

        public async Task<bool> EnsureAuthenticatedAsync()
        {
            var loggedIn = await _auth.IsLoggedInAsync();
            if (!loggedIn)
            {
                _navigation.NavigateTo("/");
                return false;
            }
            return true;
        }
    }
}
