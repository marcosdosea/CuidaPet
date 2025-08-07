namespace Core;

public partial class Petdoenca
{
    public uint Id { get; set; }

    public DateTime? DataDiagnostico { get; set; }

    public uint IdPet { get; set; }

    public uint IdDoenca { get; set; }

    public virtual Doenca IdDoencaNavigation { get; set; } = null!;

    public virtual Pet IdPetNavigation { get; set; } = null!;
}
