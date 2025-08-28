using System;
using System.Collections.Generic;

namespace Core;

public partial class Notificacao
{
    public uint Id { get; set; }

    public string Titulo { get; set; } = null!;

    public string? Descricao { get; set; }

    public DateTime DataEnvio { get; set; }

    public virtual ICollection<Pessoanotificacao> Pessoanotificacaos { get; set; } = new List<Pessoanotificacao>();
}
