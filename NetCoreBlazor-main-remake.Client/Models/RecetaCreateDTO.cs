//C:\Users\L\Documents\NetCoreBlazor-main-remake\NetCoreBlazor-main-remake.Client\Models\RecetaCreateDTO.cs
using System.ComponentModel.DataAnnotations;

namespace NetCoreBlazor_main_remake.Client.Models
{
    public class RecetaCreateDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int Type { get; set; }

        [Required]
        [Range(1, 5)]
        public int Leven { get; set; }

        [Required]
        [Range(1, 50)]
        public int QuantityPersons { get; set; }

        [Required(ErrorMessage = "Los pasos son obligatorios")]
        public string Steps { get; set; } = string.Empty;
    }
}
