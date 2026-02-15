using System.ComponentModel.DataAnnotations;
using Util;

namespace CuidaPetWeb.Models;
public class FuncionarioViewModel
{
    [Key]
    public uint Id { get; set; }

    [Display(Name = "CRMV")]
    public string? Crmv { get; set; }

    [Required(ErrorMessage = "O ID da Pessoa é obrigatório")]
    [Display(Name = "Pessoa")]
    public uint IdPessoa { get; set; }
    [Required(ErrorMessage = "O ID do Estabelecimento é obrigatório")]
    [Display(Name = "Estabelecimento")]
    public uint IdEstabelecimento { get; set; }
    [Required(ErrorMessage = "Por favor, informe um nome")]
    [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
    [Display(Name = "Nome")]
    public string Nome { get; set; } = null!;
    [Required(ErrorMessage = "Por favor, informe um CPF")]
    [StringLength(11, ErrorMessage = "O CPF deve ter no máximo 11 caracteres")]
    [Display(Name = "CPF")]
    [CPF]
    public string Cpf { get; set; } = null!;
    [EmailAddress(ErrorMessage = "Por favor, informe um email válido")]
    [Display(Name = "Email")]
    public string Email { get; set; } = null!;
    [Display(Name = "Senha")]
    public string? Senha { get; set; }
    [Required(ErrorMessage = "Por favor, informe um telefone")]
    [Display(Name = "Telefone")]
    public string? Telefone { get; set; }
    public string Tipo { get; set; } = "T";
    public string Status { get; set; } = "A";
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
    [StringLength(2, ErrorMessage = "O estado deve ter 2 caracteres", MinimumLength = 2)]
    [Display(Name = "UF")]
    public string Estado { get; set; } = null!;
}