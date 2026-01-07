using System.ComponentModel.DataAnnotations;

namespace NetCoreBlazor_main_remake.Client.Models
{
    public class PasoCreateDTO
    {
        [Required(ErrorMessage = "La descripción es obligatoria")]
        [MaxLength(1000, ErrorMessage = "Máximo 1000 caracteres")]
        public string Descripcion { get; set; } = string.Empty;
    }
}
