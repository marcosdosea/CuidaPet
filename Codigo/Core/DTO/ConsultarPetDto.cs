namespace Core.DTO;

public class ConsultarPetDto
{
    public uint IdPet { get; set; }
    public string NomePet { get; set; } = null!;
    public string Raca { get; set; } = null!;
    public string Sexo { get; set; } = null!;
    public int? Idade { get; set; }
    public string NomeTutor { get; set; } = null!;
    public List<string> Vacinas { get; set; } = new List<string>();
    public List<string> Doencas { get; set; } = new List<string>();
    public string? Observacao { get; set; }
    public uint IdAgendamento { get; set; }
    public uint IdFuncionario { get; set; }
    public uint IdTutor { get; set; }
}