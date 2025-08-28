using System.ComponentModel.DataAnnotations;
using Util;

namespace CuidaPetWeb.Models;

public class FuncionarioViewModel
{
    [Required]
    [Key]
    public uint Id { get; set; }

    //TODO - CRMV só deve ser obrigatório para veterinários
    public string Crmv { get; set; } = null!;

    [Required(ErrorMessage = "O ID da Pessoa é obrigatório")]
    [Display(Name = "Pessoa")]
    public uint IdPessoa { get; set; }

    [Required(ErrorMessage = "O ID do Estabelecimento é obrigatório")]
    [Display(Name = "Estabelecimento")]
    public uint IdEstabelecimento { get; set; }
}
