namespace Core;

public partial class Consulta
{
    public uint Id { get; set; }

    public DateTime DataConsulta { get; set; }

    public string? Anotacoes { get; set; }

    public uint IdTutor { get; set; }

    public uint IdPet { get; set; }

    public uint IdFuncionario { get; set; }

    public uint IdAgendamento { get; set; }

    public virtual Agendamento IdAgendamentoNavigation { get; set; } = null!;

    public virtual Funcionario IdFuncionarioNavigation { get; set; } = null!;

    public virtual Pet IdPetNavigation { get; set; } = null!;

    public virtual Pessoa IdTutorNavigation { get; set; } = null!;
}
