namespace Core;

public partial class Especialidade
{
    public uint Id { get; set; }

    public string Nome { get; set; } = null!;

    public string? Descricao { get; set; }

    public virtual ICollection<Funcionario> IdFuncionarios { get; set; } = new List<Funcionario>();
}
