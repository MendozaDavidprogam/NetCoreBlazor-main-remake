// NetCoreBlazor-main-remake.Client/Models/UsuarioRegister.cs

namespace NetCoreBlazor_main_remake.Client.Models
{
    public class UsuarioRegister
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "Chef";
    }
}
