using System;
using System.Collections.Generic;

namespace Core;

public partial class Pessoapet
{
    public uint IdPet { get; set; }

    public uint IdPessoa { get; set; }

    public virtual Pessoa IdPessoaNavigation { get; set; } = null!;

    public virtual Pet IdPetNavigation { get; set; } = null!;
}
