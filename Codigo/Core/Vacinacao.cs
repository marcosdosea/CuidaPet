using System;
using System.Collections.Generic;

namespace Core;

public partial class Vacinacao
{
    public uint Id { get; set; }

    public DateTime DataVacina { get; set; }

    public string? Lote { get; set; }

    public uint IdVacina { get; set; }

    public uint IdPet { get; set; }

    public uint IdFuncionario { get; set; }

    public uint IdTutor { get; set; }

    public virtual Funcionario IdFuncionarioNavigation { get; set; } = null!;

    public virtual Pet IdPetNavigation { get; set; } = null!;

    public virtual Pessoa IdTutorNavigation { get; set; } = null!;

    public virtual Vacina IdVacinaNavigation { get; set; } = null!;
}
