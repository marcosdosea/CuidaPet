using System;
using System.Collections.Generic;

namespace Core;

public partial class Pedido
{
    public uint Id { get; set; }

    /// <summary>
    /// A = Andamento, F = Finalizado, C = Cancelado
    /// </summary>
    public string Status { get; set; } = null!;

    public DateTime RealizadoEm { get; set; }

    public uint IdTutor { get; set; }

    public uint IdFuncionario { get; set; }

    public uint IdAgendamento { get; set; }

    public virtual Agendamento IdAgendamentoNavigation { get; set; } = null!;

    public virtual Funcionario IdFuncionarioNavigation { get; set; } = null!;

    public virtual Pessoa IdTutorNavigation { get; set; } = null!;

    public virtual ICollection<Pedidoproduto> Pedidoprodutos { get; set; } = new List<Pedidoproduto>();
}
