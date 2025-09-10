namespace Core.DTO;
public class AgendamentoDto
{
    public uint Id { get; set; }

    public DateTime DataSolicitacao { get; set; }

    public DateTime? DataConfirmacao { get; set; }

    public TimeSpan Horario { get; set; }

    /// <summary>
    /// S (Solicitado), A (Aprovado), C (Cancelado), R (Realizado)
    /// </summary>
    public string Status { get; set; } = null!;

    public uint IdPet { get; set; }

    public uint IdFuncionario { get; set; }

    public uint IdTutor { get; set; }

    public ICollection<Consulta> Consulta { get; set; } = new List<Consulta>();

    public string NomeFuncionario { get; set; } = null!;
    public string NomePet { get; set; } = null!;
    public string NomeRacaPet { get; set; } = null!;
    public string NomeTutor { get; set; } = null!;

    public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
