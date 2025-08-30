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
    [Display(Name = "Nome")]
    public string Nome { get; set; } = null!;
    [Required(ErrorMessage = "Por favor, informe um CPF")]
    [Display(Name = "CPF")]
    [CPF]
    public string Cpf { get; set; } = null!;
    [Display(Name = "Email")]
    public string Email { get; set; } = null!;
    [Required(ErrorMessage = "A senha é obrigatória")]
    [Display(Name = "Senha")]
    public string Senha { get; set; } = null!;
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
    [Display(Name = "UF")]
    public string Estado { get; set; } = null!;
}