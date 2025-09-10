namespace CuidaPetWeb.Models
{
    public class ConsultarItensViewModel
    {
        public string? TermoPesquisa { get; set; }
        public bool MostrarItens { get; set; } = true; // true = itens, false = petshops

        public List<EstabelecimentoComProdutosViewModel> Estabelecimentos { get; set; } = new();
    }

    public class EstabelecimentoComProdutosViewModel
    {
        public uint Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public List<ProdutoViewModel> Produtos { get; set; } = new();
    }
}