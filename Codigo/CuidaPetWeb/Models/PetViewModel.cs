using System.ComponentModel.DataAnnotations;

namespace CuidaPetWeb.Models
{
    public class PetViewModel
    {
        [Key]
        public uint Id { get; set; }

        [Required(ErrorMessage = "O nome do pet é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome do pet deve ter no máximo 100 caracteres.")]
        [Display(Name = "Nome do Pet")]
        public string Nome { get; set; } = null!;

        [Required(ErrorMessage = "O sexo do pet é obrigatório.")]
        [Display(Name = "Sexo")]
        public string Sexo { get; set; } = null!;

        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "A raça do pet é obrigatória.")]
        [Display(Name = "Raça")]
        public uint IdRaca { get; set; }
    }
}
