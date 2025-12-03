using System;
using System.Collections.Generic;

namespace Core;

public partial class Produto
{
    public uint Id { get; set; }

    public string Nome { get; set; } = null!;

    public decimal Preco { get; set; }

    /// <summary>
    /// I (Indisponível), D (Disponível), P (Promoção)
    /// </summary>
    public string? Status { get; set; }

    public decimal? PrecoPromocao { get; set; }

    public string? Descricao { get; set; }

    public uint IdCategoria { get; set; }

    public uint IdEstabelecimento { get; set; }

    public virtual Categoria IdCategoriaNavigation { get; set; } = null!;

    public virtual Estabelecimento IdEstabelecimentoNavigation { get; set; } = null!;

    public virtual ICollection<Pedidoproduto> Pedidoprodutos { get; set; } = new List<Pedidoproduto>();
}
