namespace Core;

public partial class Doenca
{
    public uint Id { get; set; }

    public string Nome { get; set; } = null!;

    public uint IdEspecie { get; set; }

    public virtual Especie IdEspecieNavigation { get; set; } = null!;

    public virtual ICollection<Petdoenca> Petdoencas { get; set; } = new List<Petdoenca>();

    public virtual ICollection<Vacina> Vacinas { get; set; } = new List<Vacina>();
}
