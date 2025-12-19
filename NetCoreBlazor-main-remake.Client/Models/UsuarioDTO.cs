// NetCoreBlazor-main-remake.Client/Models/UsuarioDTO.cs

namespace NetCoreBlazor_main_remake.Client.Models
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
