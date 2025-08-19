using System.ComponentModel.DataAnnotations;

namespace CuidaPetWeb.Models
{
    public class DoencaViewModel
    {
        [Required]
        [Key]
        public uint Id { get; set; }
        [Required(ErrorMessage = "O nome da doença é obrigatório.")]
        [Display(Name = "Nome da Doença")]

        public string Nome { get; set; } = null!;
        [Required(ErrorMessage = "A descrição da doença é obrigatória.")]
        [Display(Name = "Descrição da Doença")]
        public uint IdEspecie { get; set; }
    }
}
