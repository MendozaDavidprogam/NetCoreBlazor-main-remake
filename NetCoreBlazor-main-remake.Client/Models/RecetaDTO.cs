//C:\Users\L\Documents\NetCoreBlazor-main-remake\NetCoreBlazor-main-remake.Client\Models\RecetaDTO.cs
namespace NetCoreBlazor_main_remake.Client.Models
{
    public class RecetaDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Type { get; set; }
        public int Leven { get; set; }
        public int QuantityPersons { get; set; }
        public string Steps { get; set; } = string.Empty;
        public bool Visibility { get; set; }
    }
}
