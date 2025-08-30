namespace Core;

public partial class Horariosatendimento
{
    public uint Id { get; set; }

    /// <summary>
    /// 
    /// 
    /// </summary>
    public string DiaSemana { get; set; } = null!;

    public TimeSpan Horario { get; set; }

    public uint IdFuncionario { get; set; }

    public virtual Funcionario IdFuncionarioNavigation { get; set; } = null!;
}
