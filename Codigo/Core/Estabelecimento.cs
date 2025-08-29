using System;
using System.Collections.Generic;

namespace Core;

public partial class Estabelecimento
{
    public uint Id { get; set; }

    public string Nome { get; set; } = null!;

    /// <summary>
    /// C(Clínica), P(Petshop), A(Ambos)
    /// </summary>
    public string? Tipo { get; set; }

    public string Cnpj { get; set; } = null!;

    public string Telefone { get; set; } = null!;

    public string Logradouro { get; set; } = null!;

    public string Numero { get; set; } = null!;

    public string? Complemento { get; set; }

    public string Bairro { get; set; } = null!;

    public string Cidade { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public uint IdGerente { get; set; }

    public virtual ICollection<Funcionario> Funcionarios { get; set; } = new List<Funcionario>();

    public virtual Pessoa IdGerenteNavigation { get; set; } = null!;

    public virtual ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}
