namespace CuidaPetWeb.Models
{
    public class DetalhesEstabelecimentoViewModel
    {
        public uint Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Logradouro { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public List<ProdutoViewModel> Produtos { get; set; } = new();
    }
}
