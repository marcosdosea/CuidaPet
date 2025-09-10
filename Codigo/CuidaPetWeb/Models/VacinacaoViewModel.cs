using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CuidaPetWeb.Models
{
    public class VacinacaoViewModel
    {
        [Key]
        public uint Id { get; set; }

        [Display(Name = "Data da Vacinação")]
        [Required(ErrorMessage = "A data da vacinação é obrigatória.")]
        [DataType(DataType.Date)]
        public DateTime DataVacina { get; set; }

        [Display(Name = "Lote")]
        [StringLength(50, ErrorMessage = "O lote deve ter no máximo 50 caracteres.")]
        public string? Lote { get; set; }

        [Display(Name = "Vacina")]
        [Required(ErrorMessage = "É obrigatório selecionar uma vacina.")]
        public uint IdVacina { get; set; }

        [Display(Name = "Pet")]
        [Required(ErrorMessage = "É obrigatório selecionar um pet.")]
        public uint IdPet { get; set; }

        [Display(Name = "Funcionário")]
        [Required(ErrorMessage = "É obrigatório selecionar um funcionário.")]
        public uint IdFuncionario { get; set; }

        [Display(Name = "Tutor")]
        [Required(ErrorMessage = "É obrigatório selecionar um tutor.")]
        public uint IdTutor { get; set; }

        public IEnumerable<SelectListItem>? Vacinas { get; set; }
        public IEnumerable<SelectListItem>? Pets { get; set; }
        public IEnumerable<SelectListItem>? Funcionarios { get; set; }
        public IEnumerable<SelectListItem>? Tutores { get; set; }
    }
}
