namespace Core;

public partial class Pedidoproduto
{
    public uint Id { get; set; }

    public int Quantidade { get; set; }

    public decimal Preco { get; set; }

    public uint IdProduto { get; set; }

    public uint IdPedido { get; set; }

    public virtual Pedido IdPedidoNavigation { get; set; } = null!;

    public virtual Produto IdProdutoNavigation { get; set; } = null!;
}
