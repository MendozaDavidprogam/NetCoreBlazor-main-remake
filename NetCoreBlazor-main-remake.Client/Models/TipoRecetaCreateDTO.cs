using System.ComponentModel.DataAnnotations;

namespace NetCoreBlazor_main_remake.Client.Models
{
    public class TipoRecetaCreateDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(255, ErrorMessage = "Máximo 255 caracteres")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [MaxLength(500, ErrorMessage = "Máximo 500 caracteres")]
        public string Descriptions { get; set; } = string.Empty;
    }
}
