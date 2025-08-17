using System.ComponentModel.DataAnnotations;

namespace CuidaPetWeb.Models
{
    public class RacaViewModel
    {
        [Key]
        public uint Id { get; set; }

        [Required(ErrorMessage = "O nome da raça é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome da raça deve ter no máximo 100 caracteres.")]
        [Display(Name = "Nome da Raça")]
        public string Nome { get; set; } = null!;

        [Required(ErrorMessage = "A espécie é obrigatória.")]
        [Display(Name = "Espécie")]
        public uint IdEspecie { get; set; }
    }
}
