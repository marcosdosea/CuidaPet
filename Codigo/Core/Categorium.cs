using System;
using System.Collections.Generic;

namespace Core;

public partial class Categorium
{
    public uint Id { get; set; }

    public string Nome { get; set; } = null!;

    public string? Descricao { get; set; }

    public virtual ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}
