namespace Core;

public partial class Pessoanotificacao
{
    public int Id { get; set; }

    /// <summary>
    /// 0 - Não lida, 1 - Lida
    /// </summary>
    public sbyte StatusLida { get; set; }

    public uint IdPessoa { get; set; }

    public uint IdNotificacao { get; set; }

    public virtual Notificacao IdNotificacaoNavigation { get; set; } = null!;

    public virtual Pessoa IdPessoaNavigation { get; set; } = null!;
}
