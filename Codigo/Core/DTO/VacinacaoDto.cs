namespace Core.DTO;

public class VacinacaoDto
{
    public uint Id { get; set; }
    public DateTime DataVacina { get; set; }
    public string? Lote { get; set; }
    public uint IdVacina { get; set; }
    public uint IdPet { get; set; }
    public uint IdFuncionario { get; set; }
    public uint IdTutor { get; set; }
}
