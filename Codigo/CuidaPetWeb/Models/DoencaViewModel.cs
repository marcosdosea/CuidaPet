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
        [Required(ErrorMessage = "o ID da espécie é obrigatório.")]
        [Display(Name = "ID da espécie")]
        public uint IdEspecie { get; set; }
    }
}
