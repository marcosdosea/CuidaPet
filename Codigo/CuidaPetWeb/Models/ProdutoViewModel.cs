using System.ComponentModel.DataAnnotations;

namespace CuidaPetWeb.Models
{
    public class ProdutoViewModel
    {
        [Required]
        [Key]
        public uint Id { get; set; }

        [Required(ErrorMessage = "O nome do produto é obrigatório.")]
        [Display(Name = "Nome do Produto")]
        public string Nome { get; set; } = null!;

        [Required(ErrorMessage = "O preço do produto é obrigatório.")]
        [Display(Name = "Preço do Produto")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero.")]
        public decimal Preco { get; set; }

        /// <summary>
        /// I (Indisponível), D (Disponível), P (Promoção)
        /// </summary>
        [Display(Name = "Status do Produto")]
        public string? Status { get; set; }

        [Display(Name = "Preço de Promoção")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço promocional deve ser maior que zero.")]
        public decimal? PrecoPromocao { get; set; }

        [Display(Name = "Descrição do Produto")]
        public string? Descricao { get; set; }


        [Required(ErrorMessage = "A categoria do produto é obrigatória.")]
        [Display(Name = "Categoria do Produto")]
        public uint IdCategoria { get; set; }

        [Required(ErrorMessage = "O estabelecimento do produto é obrigatório.")]
        [Display(Name = "Estabelecimento do Produto")]
        public uint IdEstabelecimento { get; set; }

        public string Categoria { get; set; } = string.Empty;
    }
}
