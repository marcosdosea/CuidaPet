namespace Core;

public partial class Funcionario
{
    public uint Id { get; set; }

    public string Crmv { get; set; } = null!;

    public string Cpf { get; set; } = null!;

    public uint IdPessoa { get; set; }

    public uint IdEstabelecimento { get; set; }

    public virtual ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();

    public virtual ICollection<Consulta> Consulta { get; set; } = new List<Consulta>();

    public virtual ICollection<Horariosatendimento> Horariosatendimentos { get; set; } = new List<Horariosatendimento>();

    public virtual Estabelecimento IdEstabelecimentoNavigation { get; set; } = null!;

    public virtual Pessoa IdPessoaNavigation { get; set; } = null!;

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    public virtual ICollection<Vacinacao> Vacinacaos { get; set; } = new List<Vacinacao>();

    public virtual ICollection<Especialidade> IdEspecialidades { get; set; } = new List<Especialidade>();
}
