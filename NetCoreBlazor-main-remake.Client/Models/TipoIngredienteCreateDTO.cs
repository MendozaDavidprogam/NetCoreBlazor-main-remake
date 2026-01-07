//  NetCoreBlazor-main-remake.Client/Models/TipoIngredienteCreateDTO.cs
using System.ComponentModel.DataAnnotations;

namespace NetCoreBlazor_main_remake.Client.Models
{
    public class TipoIngredienteCreateDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [MaxLength(500)]
        public string Descriptions { get; set; } = string.Empty;
    }
}
