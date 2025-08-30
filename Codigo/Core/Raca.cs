namespace Core;

public partial class Raca
{
    public uint Id { get; set; }

    public string Nome { get; set; } = null!;

    public uint IdEspecie { get; set; }

    public virtual Especie IdEspecieNavigation { get; set; } = null!;

    public virtual ICollection<Pet> Pets { get; set; } = new List<Pet>();
}
