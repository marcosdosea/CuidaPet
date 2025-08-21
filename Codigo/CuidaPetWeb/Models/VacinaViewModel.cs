using System.ComponentModel.DataAnnotations;
using Core;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CuidaPetWeb.Models
{
    public class VacinaViewModel
    {
        [Key]
        public uint Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Display(Name = "Período de Reforço (em dias)")]
        [Range(1, ushort.MaxValue, ErrorMessage = "O período deve ser um número positivo.")]
        public ushort? PeriodoEmDias { get; set; }

        [Display(Name = "Doença")]
        [Required(ErrorMessage = "É obrigatório selecionar uma doença.")]
        public uint IdDoenca { get; set; }

        [Display(Name = "Espécie")]
        [Required(ErrorMessage = "É obrigatório selecionar uma espécie.")]
        public uint IdEspecie { get; set; }

        public IEnumerable<SelectListItem>? Doencas { get; set; }
        public IEnumerable<SelectListItem>? Especies { get; set; }
    }
}
