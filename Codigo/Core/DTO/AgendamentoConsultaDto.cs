namespace Core.DTO;

public class AgendamentoConsultaDto
{
    public uint IdAgendamento { get; set; }
    public int Numero { get; set; }
    public string NomeTutor { get; set; } = null!;
    public string NomePet { get; set; } = null!;
    public TimeSpan Horario { get; set; }
    public uint IdPet { get; set; }
}