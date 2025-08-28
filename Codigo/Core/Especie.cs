using System;
using System.Collections.Generic;

namespace Core;

public partial class Especie
{
    public uint Id { get; set; }

    public string Nome { get; set; } = null!;

    public virtual ICollection<Doenca> Doencas { get; set; } = new List<Doenca>();

    public virtual ICollection<Raca> Racas { get; set; } = new List<Raca>();

    public virtual ICollection<Vacina> Vacinas { get; set; } = new List<Vacina>();
}
