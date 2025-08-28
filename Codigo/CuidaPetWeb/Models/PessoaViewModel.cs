using System.ComponentModel.DataAnnotations;

namespace CuidaPetWeb.Models
{
    public class PessoaViewModel
    {
        [Required]
        [Key]
        public uint Id { get; set; }
        [Required(ErrorMessage = "Por favor, informe um nome")]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = null!;
        [Required(ErrorMessage = "Por favor, informe um email")]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "A senha é obrigatória")]
        [Display(Name = "Senha")]
        public string Senha { get; set; } = null!;
        [Display(Name = "Telefone")]
        public string? Telefone { get; set; }
        /// <summary>
        /// T (Tutor), G (Gerente), A (Atendente), V (Veterinário), Ad (Administrador)
        /// </summary>
        [Required]
        public string Tipo { get; set; } = "T";
        /// <summary>
        /// A (Ativo), I (Inativo)
        /// </summary>
        [Required]
        public string Status { get; set; } = null!;
        [Required(ErrorMessage = "Por favor, informe seu endereço")]
        [Display(Name = "Endereço")]
        public string Logradouro { get; set; } = null!;
        [Required(ErrorMessage = "Por favor, informe seu número de endereço")]
        [Display(Name = "nº")]
        public string Numero { get; set; } = null!;
        [Display(Name = "Complemento")]
        public string? Complemento { get; set; }
        [Required(ErrorMessage = "Por favor, informe seu bairro")]
        [Display(Name = "Bairro")]
        public string Bairro { get; set; } = null!;
        [Required(ErrorMessage = "Por favor, informe sua cidade")]
        [Display(Name = "Cidade")]
        public string Cidade { get; set; } = null!;
        [Required(ErrorMessage = "Por favor, informe seu Estado")]
        [Display(Name = "UF")]
        public string Estado { get; set; } = null!;
    }
}
