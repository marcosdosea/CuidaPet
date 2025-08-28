using System;
using System.Collections.Generic;

namespace Core;

public partial class Agendamento
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

    public virtual ICollection<Consulta> Consulta { get; set; } = new List<Consulta>();

    public virtual Funcionario IdFuncionarioNavigation { get; set; } = null!;

    public virtual Pet IdPetNavigation { get; set; } = null!;

    public virtual Pessoa IdTutorNavigation { get; set; } = null!;

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
