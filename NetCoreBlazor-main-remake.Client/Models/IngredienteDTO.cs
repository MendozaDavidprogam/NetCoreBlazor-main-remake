namespace NetCoreBlazor_main_remake.Client.Models
{
    public class IngredienteDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Descriptions { get; set; } = string.Empty;

        public int TipoIngredienteId { get; set; }
        public string TipoIngredienteNombre { get; set; } = string.Empty;
    }
}
