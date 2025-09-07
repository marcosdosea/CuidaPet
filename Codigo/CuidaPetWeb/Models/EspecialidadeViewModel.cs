using System.ComponentModel.DataAnnotations;

namespace CuidaPetWeb.Models
{
    public class EspecialidadeViewModel
    {
        [Key]
        public uint Id { get; set; }
        [Display(Name = "Nome da Especialidade")]
        
        public string Nome { get; set; } = null!;
        [Required(ErrorMessage = "O nome da especialidade é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]

        public string Descricao { get; set; } = null!;
    }
}
