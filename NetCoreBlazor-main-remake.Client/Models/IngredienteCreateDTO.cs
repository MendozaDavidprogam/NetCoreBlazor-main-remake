using System.ComponentModel.DataAnnotations;

namespace NetCoreBlazor_main_remake.Client.Models
{
    public class IngredienteCreateDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un tipo de ingrediente")]
        public int Type { get; set; }
    }
}
