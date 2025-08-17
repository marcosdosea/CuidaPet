namespace Core.DTO
{
    public class Raca
    {
        public uint Id { get; set; }
        public string Nome { get; set; } = null!;
        public uint IdEspecie { get; set; }
        public string? Especie { get; set; }
    }
}
