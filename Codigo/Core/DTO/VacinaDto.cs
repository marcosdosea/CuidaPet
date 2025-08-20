namespace Core.DTO;
public class VacinaDTO
{
    public uint Id { get; set; }
    public string Nome { get; set; } = null!;
    public ushort? PeriodoEmDias { get; set; }
    public string Doenca { get; set; } = null!;
    public string Especie { get; set; } = null!;
}