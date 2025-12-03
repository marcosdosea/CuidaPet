using System;
using System.Collections.Generic;

namespace Core;

public partial class Pet
{
    public uint Id { get; set; }

    public string Nome { get; set; } = null!;

    /// <summary>
    /// M (Macho), F (Fêmea)
    /// </summary>
    public string Sexo { get; set; } = null!;

    public DateTime? DataNascimento { get; set; }

    public uint IdRaca { get; set; }

    public virtual ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();

    public virtual ICollection<Consulta> Consulta { get; set; } = new List<Consulta>();

    public virtual Raca IdRacaNavigation { get; set; } = null!;

    public virtual ICollection<Petdoenca> Petdoencas { get; set; } = new List<Petdoenca>();

    public virtual ICollection<Vacinacao> Vacinacaos { get; set; } = new List<Vacinacao>();
}
