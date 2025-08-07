namespace Core;

public partial class Pessoa
{
    public uint Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Senha { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Telefone { get; set; }

    /// <summary>
    /// T (Tutor), G (Gerente), A (Atendente), V (Veterinário), Ad (Administrador)
    /// </summary>
    public string Tipo { get; set; } = null!;

    /// <summary>
    /// A (Ativo), I (Inativo)
    /// </summary>
    public string Status { get; set; } = null!;

    public string Logradouro { get; set; } = null!;

    public string Numero { get; set; } = null!;

    public string? Complemento { get; set; }

    public string Bairro { get; set; } = null!;

    public string Cidade { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public virtual ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();

    public virtual ICollection<Consulta> Consulta { get; set; } = new List<Consulta>();

    public virtual ICollection<Estabelecimento> Estabelecimentos { get; set; } = new List<Estabelecimento>();

    public virtual Funcionario? Funcionario { get; set; }

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    public virtual ICollection<Pessoanotificacao> Pessoanotificacaos { get; set; } = new List<Pessoanotificacao>();

    public virtual ICollection<Vacinacao> Vacinacaos { get; set; } = new List<Vacinacao>();
}
