using System.ComponentModel.DataAnnotations;

namespace CuidaPetWeb.Models
{
    public class EstabelecimentoViewModel
    {
        [Key]
        public uint Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;
        [Display(Name = "Tipo")]
        [StringLength(50, ErrorMessage = "O tipo deve ter no máximo 50 caracteres.")]
        public string? Tipo { get; set; }
        [Display(Name = "CNPJ")]
        [Required(ErrorMessage = "O campo CNPJ é obrigatório.")]
        [StringLength(20, ErrorMessage = "O CNPJ deve ter no máximo 20 caracteres.")]
        public string Cnpj { get; set; } = string.Empty;
        [Display(Name = "Telefone")]
        [Required(ErrorMessage = "O campo Telefone é obrigatório.")]
        [StringLength(15, ErrorMessage = "O telefone deve ter no máximo 15 caracteres.")]
        public string Telefone { get; set; } = string.Empty;
        [Display(Name = "Logradouro")]
        [Required(ErrorMessage = "O campo Logradouro é obrigatório.")]
        [StringLength(100, ErrorMessage = "O logradouro deve ter no máximo 100 caracteres.")]
        public string Logradouro { get; set; } = string.Empty;
        [Display(Name = "Número")]
        [Required(ErrorMessage = "O campo Número é obrigatório.")]
        [StringLength(10, ErrorMessage = "O número deve ter no máximo 10 caracteres.")]
        public string Numero { get; set; } = string.Empty;
        [Display(Name = "Complemento")]
        [StringLength(50, ErrorMessage = "O complemento deve ter no máximo 50 caracteres.")]
        public string? Complemento { get; set; }
        [Display(Name = "Bairro")]
        [Required(ErrorMessage = "O campo Bairro é obrigatório.")]
        [StringLength(50, ErrorMessage = "O bairro deve ter no máximo 50 caracteres.")]
        public string Bairro { get; set; } = string.Empty;
        [Display(Name = "Cidade")]
        [Required(ErrorMessage = "O campo Cidade é obrigatório.")]
        [StringLength(50, ErrorMessage = "A cidade deve ter no máximo 50 caracteres.")]
        public string Cidade { get; set; } = string.Empty;
        [Display(Name = "Estado")]
        [Required(ErrorMessage = "O campo Estado é obrigatório.")]
        [StringLength(2, ErrorMessage = "O estado deve ter no máximo 2 caracteres.")]
        public string Estado { get; set; } = string.Empty;
        [Display(Name = "Gerente")]
        [Required(ErrorMessage = "É obrigatório selecionar um gerente.")]
        public uint IdGerente { get; set; }

        [Display(Name = "Nome do Gerente")]
        public string? NomeGerente { get; set; }
    }
}