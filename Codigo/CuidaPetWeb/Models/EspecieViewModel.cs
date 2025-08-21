using System.ComponentModel.DataAnnotations;

namespace CuidaPetWeb.Models
{
    public class EspecieViewModel
    {
        [Key]
        public uint Id { get; set; }

        [Required(ErrorMessage = "O nome da espécie é obrigatório.")]
        [StringLength(100)]
        public string Nome { get; set; } = null!;

    }
}
