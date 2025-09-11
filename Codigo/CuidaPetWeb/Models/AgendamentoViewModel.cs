using System.ComponentModel.DataAnnotations;

namespace CuidaPetWeb.Models
{
    public class AgendamentoViewModel
    {
        [Key]
        public uint Id { get; set; }

        [Required(ErrorMessage = "A data da solicitação é obrigatória.")]
        [DataType(DataType.DateTime)]
        public DateTime DataSolicitacao { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DataConfirmacao { get; set; }

        [Required(ErrorMessage = "O horário é obrigatório.")]
        [DataType(DataType.Time)]
        public TimeSpan Horario { get; set; }

        [Required(ErrorMessage = "O status é obrigatório.")]
        [StringLength(1, ErrorMessage = "Status deve ter 1 caractere.")]
        public string Status { get; set; } = null!;

        [Required(ErrorMessage = "O Pet é obrigatório.")]
        public uint IdPet { get; set; }

        [Required(ErrorMessage = "O Funcionário é obrigatório.")]
        public uint IdFuncionario { get; set; }

        [Required(ErrorMessage = "O Tutor é obrigatório.")]
        public uint IdTutor { get; set; }

        // Propriedades auxiliares para exibição (opcional)
        public string? NomePet { get; set; }
        public string? NomeFuncionario { get; set; }
        public string? NomeTutor { get; set; }
    }
}