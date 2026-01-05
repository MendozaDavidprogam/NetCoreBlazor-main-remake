using System.ComponentModel.DataAnnotations;

namespace NetCoreBlazor_main_remake.Client.Models
{
    public class UnidadMedicionCreateDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(255, ErrorMessage = "Máximo 255 caracteres")]
        public string Name { get; set; } = string.Empty;
    }
}
