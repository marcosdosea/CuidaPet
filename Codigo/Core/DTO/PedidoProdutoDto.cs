using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class PedidoProdutoDto
    {
        public uint Id { get; set; }
        public uint PedidoId { get; set; }
        public uint ProdutoId { get; set; }
        public DateTime RealizadoEm { get; set; }
        public string Status { get; set; } = null!;
        public string ProdutoNome { get; set; } = null!;
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal PrecoTotal { get; set; }
        public uint TutorId { get; set; }
        public string TutorNome { get; set; } = null!;
        public string? TutorTelefone { get; set; }
    }
}
