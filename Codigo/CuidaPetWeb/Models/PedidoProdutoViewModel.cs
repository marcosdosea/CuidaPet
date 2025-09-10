using System;
using System.ComponentModel.DataAnnotations;

namespace CuidaPetWeb.Models
{
    public class PedidoProdutoViewModel
    {
        [Key]
        public uint Id { get; set; }

        [Display(Name = "Data/Hora do Pedido")]
        public DateTime RealizadoEm { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; } = null!;

        [Display(Name = "Nome do Produto")]
        public string ProdutoNome { get; set; } = null!;

        [Display(Name = "Quantidade")]
        public int Quantidade { get; set; }

        [Display(Name = "Valor Total")]
        [DataType(DataType.Currency)]
        public decimal PrecoTotal { get; set; }

        [Display(Name = "Nome do Tutor")]
        public string TutorNome { get; set; } = null!;

        [Display(Name = "Telefone do Tutor")]
        public string? TutorTelefone { get; set; }
        public string FormattedTelefone => !string.IsNullOrEmpty(TutorTelefone) ? $"https://wa.me/{TutorTelefone}" : string.Empty;
    }
}
