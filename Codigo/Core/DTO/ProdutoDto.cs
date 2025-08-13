namespace Core.DTO
{
    public class ProdutoDTO
    {
        public uint Id { get; set; }
        public string Nome { get; set; } = null!;
        public decimal Preco { get; set; }
        public string? Status { get; set; }
        public decimal? PrecoPromocao { get; set; }
        public string? Descricao { get; set; }
        public string Categoria { get; set; } = null!;
        public string Estabelecimento { get; set; } = null!;
    }
}
