// NetCoreBlazor-main-remake.Client/Models/UsuarioRegister.cs

using System.ComponentModel.DataAnnotations;

namespace NetCoreBlazor_main_remake.Client.Models
{
    public class UsuarioRegister
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Lastname { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;

        public string Role { get; set; } = "Chef";
    }
}
