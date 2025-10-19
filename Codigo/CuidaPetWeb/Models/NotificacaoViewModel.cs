using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CuidaPetWeb.Models
{
    public class NotificacaoViewModel
    {
        [Key]
        public uint Id { get; set; }

        [Required(ErrorMessage = "O título da notificação é obrigatório.")]
        [StringLength(45)]
        [DisplayName("Título")]
        public string Titulo { get; set; } = null!;

        [StringLength(150)]
        [DisplayName("Descrição")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "A data de envio da notificação é obrigatória.")]
        [DisplayName("Data de envio da notificação")]
        public DateTime DataEnvio { get; set; }

        [DisplayName("Status")]
        public sbyte? StatusLida { get; set; }

        public bool EstaLida => StatusLida.HasValue && StatusLida.Value == 1;
    }
}
