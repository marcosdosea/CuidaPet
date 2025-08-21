namespace Core.DTO;
public class VacinaDTO
{
    public uint Id { get; set; }
    public string Nome { get; set; } = null!;
    public ushort? PeriodoEmDias { get; set; }
    public Doenca Doenca { get; set; } = null!;
    public Especie Especie { get; set; } = null!;
}